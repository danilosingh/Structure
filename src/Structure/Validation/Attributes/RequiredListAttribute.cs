using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Structure.Validation.Attributes
{
    public sealed class RequiredListAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is IEnumerable enumerable)
            {
                return enumerable.OfType<object>()
                    .Where(c => c != null)
                    .Count() > 0;
            }

            return false;
        }
    }
}
