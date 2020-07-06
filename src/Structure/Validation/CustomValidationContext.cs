using System.Collections.Generic;

namespace Structure.Validation
{
    public class CustomValidationContext
    {
        public List<ValidationError> Results { get; }

        public CustomValidationContext(List<ValidationError> results)
        {
            Results = results;
        }
    }
}