using System.Collections.Generic;

namespace Structure.Validation.Interception
{
    public class CustomValidator : IMethodParameterValidator
    {
        public CustomValidator()
        { }

        public IEnumerable<ValidationError> Validate(object validatingObject)
        {
            var validationErrors = new List<ValidationError>();

            if (validatingObject is ICustomValidate customValidateObject)
            {
                var context = new CustomValidationContext(validationErrors);
                customValidateObject.AddValidationErrors(context);
            }

            return validationErrors;
        }
    }
}
