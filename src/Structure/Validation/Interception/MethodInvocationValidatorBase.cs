using Structure.Collections;
using Structure.Collections.Extensions;
using Structure.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Structure.Validation.Interception
{
    public abstract class MethodInvocationValidatorBase : IMethodInvocationValidator
    {
        private const int MaxRecursiveParameterValidationDepth = 8;

        private readonly IServiceProvider serviceProvider;
        private readonly ITypeList<IMethodParameterValidator> validators;

        protected List<IShouldNormalize> ObjectsToBeNormalized { get; }

        public MethodInvocationValidatorBase(IServiceProvider serviceProvider)
        {
            ObjectsToBeNormalized = new List<IShouldNormalize>();
            validators = new TypeList<IMethodParameterValidator>();
            this.serviceProvider = serviceProvider;
        }

        public IList<ValidationError> Validate(MethodInfo method, object[] parameterValues)
        {
            var results = new List<ValidationError>();
            var parameters = method.GetParameters();

            if (parameters.IsNullOrEmpty())
            {
                return results;
            }

            if (!method.IsPublic)
            {
                return results;
            }

            if (IsValidationDisabled(method))
            {
                return results;
            }

            if (parameters.Length != parameterValues.Length)
            {
                throw new StructureException("Method parameter count does not match with argument count!");
            }

            for (var i = 0; i < parameters.Length; i++)
            {
                ValidateMethodParameter(results, parameters[i], parameterValues[i]);
            }

            if (!results.Any())
            {
                foreach (var objectToBeNormalized in ObjectsToBeNormalized)
                {
                    objectToBeNormalized.Normalize();
                }
            }

            return results;
        }

        protected virtual bool IsValidationDisabled(MethodInfo method)
        {
            if (method.IsDefined(typeof(EnableValidationAttribute), true))
            {
                return false;
            }

            return TypeHelper.GetAttributeOfMemberOrDeclaringTypeOrDefault<DisableValidationAttribute>(method) != null;
        }

        protected virtual void ValidateMethodParameter(IList<ValidationError> errors, ParameterInfo parameterInfo, object parameterValue)
        {
            if (parameterValue == null)
            {
                if (!parameterInfo.IsOptional &&
                    !parameterInfo.IsOut &&
                    !TypeHelper.IsPrimitiveExtendedIncludingNullable(parameterInfo.ParameterType, includeEnums: true))
                {
                    errors.Add(new ValidationError(parameterInfo.Name + " is null!", parameterInfo.Name));
                }

                return;
            }

            ValidateObjectRecursively(errors, parameterValue, 1);
        }

        protected virtual void ValidateObjectRecursively(IList<ValidationError> results, object validatingObject, int currentDepth)
        {
            if (currentDepth > MaxRecursiveParameterValidationDepth)
            {
                return;
            }

            if (validatingObject == null)
            {
                return;
            }

            if (TypeHelper.IsPrimitiveExtendedIncludingNullable(validatingObject.GetType()))
            {
                return;
            }

            SetValidationErrors(results, validatingObject);

            if (IsEnumerable(validatingObject))
            {
                foreach (var item in (IEnumerable)validatingObject)
                {
                    ValidateObjectRecursively(results, item, currentDepth + 1);
                }
            }

            if (validatingObject is IShouldNormalize)
            {
                ObjectsToBeNormalized.Add(validatingObject as IShouldNormalize);
            }

            if (ShouldMakeDeepValidation(validatingObject))
            {
                var properties = TypeDescriptor.GetProperties(validatingObject).Cast<PropertyDescriptor>();
                foreach (var property in properties)
                {
                    if (property.Attributes.OfType<DisableValidationAttribute>().Any())
                    {
                        continue;
                    }

                    ValidateObjectRecursively(results, property.GetValue(validatingObject), currentDepth + 1);
                }
            }
        }

        protected virtual void SetValidationErrors(IList<ValidationError> results, object validatingObject)
        {
            foreach (var validatorType in validators)
            {
                if (!ShouldValidateUsingValidator(validatingObject, validatorType))
                {
                    continue;
                }

                var validator = serviceProvider.GetService(validatorType) as IMethodParameterValidator;
                var validationResults = validator.Validate(validatingObject);
                results.AddRange(validationResults);
            }
        }

        protected virtual bool ShouldValidateUsingValidator(object validatingObject, Type validatorType)
        {
            return true;
        }

        protected virtual bool ShouldMakeDeepValidation(object validatingObject)
        {
            // Do not recursively validate for enumerable objects
            if (validatingObject is IEnumerable)
            {
                return false;
            }

            var validatingObjectType = validatingObject.GetType();

            // Do not recursively validate for primitive objects
            if (TypeHelper.IsPrimitiveExtendedIncludingNullable(validatingObjectType))
            {
                return false;
            }

            return true;
        }

        private bool IsEnumerable(object validatingObject)
        {
            return
                validatingObject is IEnumerable &&
                !(validatingObject is IQueryable) &&
                !TypeHelper.IsPrimitiveExtendedIncludingNullable(validatingObject.GetType());
        }
    }
}
