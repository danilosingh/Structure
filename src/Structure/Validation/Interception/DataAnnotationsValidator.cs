using Structure.Collections.Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Structure.Validation.Interception
{
    public class DataAnnotationsValidator : IMethodParameterValidator
    {
        public virtual IEnumerable<ValidationError> Validate(object validatingObject)
        {
            return GetDataAnnotationAttributeErrors(validatingObject)
                .Select(c => new ValidationError(c.ErrorMessage, c.MemberNames.FirstOrDefault()));
        }

        protected virtual List<ValidationResult> GetDataAnnotationAttributeErrors(object validatingObject)
        {
            var validationResults = new List<ValidationResult>();
            var properties = TypeDescriptor.GetProperties(validatingObject).Cast<PropertyDescriptor>();

            foreach (var property in properties)
            {
                var validationAttributes = property.Attributes.OfType<ValidationAttribute>().ToArray();

                if (validationAttributes.IsNullOrEmpty())
                {
                    continue;
                }

                var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(validatingObject)
                {
                    DisplayName = property.DisplayName,
                    MemberName = property.Name
                };
                
                foreach (var attribute in validationAttributes)
                {
                    var result = attribute.GetValidationResult(property.GetValue(validatingObject), validationContext);

                    if (result != null)
                    {
                        validationResults.Add(result);
                    }
                }
            }

            return validationResults;
        }
    }
}
