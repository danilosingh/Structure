using System.Linq;

namespace Structure.Validation
{
    public class ValidatorResult
    {
        public ValidationErrorCollection Errors { get; private set; }
        public bool IsValid { get { return !Errors.Any(); } }

        public ValidatorResult()
        {
            Errors = new ValidationErrorCollection();
        }
    }
}
