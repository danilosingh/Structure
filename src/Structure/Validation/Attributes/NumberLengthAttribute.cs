using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Structure.Validation.Attributes
{
    public sealed class NumberLengthAttribute : ValidationAttribute
    {
        public int Length { get; }

        public NumberLengthAttribute(int length)
        {
            Length = length;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            var numbers = Regex.Replace(value.ToString(), "[^0-9]", "");

            if (numbers.Length == Length)
            {
                return true;
            }

            return false;
        }
    }
}
