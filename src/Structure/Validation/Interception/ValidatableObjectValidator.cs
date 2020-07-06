using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Structure.Validation.Interception
{
    public class ValidatableObjectValidator : IMethodParameterValidator
    {
        public virtual IEnumerable<ValidationError> Validate(object validatingObject)
        {
            var validationErrors = new List<ValidationError>();

            if (validatingObject is IValidatableObject validatableObject)
            {
                validationErrors.AddRange(
                    validatableObject.Validate(new System.ComponentModel.DataAnnotations.ValidationContext(validatableObject))
                        .Select(c => new ValidationError(c.ErrorMessage, c.MemberNames.FirstOrDefault())));
            }

            return validationErrors;
        }
    }
}
