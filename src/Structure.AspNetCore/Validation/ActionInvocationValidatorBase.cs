using Structure.DependencyInjection;
using Structure.Validation;
using Structure.Validation.Interception;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Structure.AspNetCore.Validation
{
    public abstract class ActionInvocationValidatorBase : MethodInvocationValidatorBase
    {
        protected IList<Type> ValidatorsToSkip => new List<Type>
        {
            typeof(DataAnnotationsValidator),
            typeof(ValidatableObjectValidator)
        };

        protected ActionInvocationValidatorBase(IServiceProvider serviceProvider) : base(serviceProvider)
        { }

        protected override bool ShouldValidateUsingValidator(object validatingObject, Type validatorType)
        {
            // Skip DataAnnotations and IValidatableObject validation because MVC does this automatically
            if (ValidatorsToSkip.Contains(validatorType))
            {
                return false;
            }

            return base.ShouldValidateUsingValidator(validatingObject, validatorType);
        }

        protected override void ValidateMethodParameter(IList<ValidationError> errors, ParameterInfo parameterInfo, object parameterValue)
        {
            if (parameterValue != null)
            {
                base.ValidateMethodParameter(errors, parameterInfo, parameterValue);
            }
        }
    }
}
