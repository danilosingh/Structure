using System;

namespace Structure.Extensions
{
    public static class GuidExtensions
    {
        public static bool EqualsAndNotEmpty(this Guid value1, Guid value2)
        {
            return value1 != Guid.Empty && value1 == value2;
        }

        public static bool IsNullOrEmpty(this Guid? value)
        {
            return value == null || value.Value == Guid.Empty; 
        }

        public static bool IsEmpty(this Guid value)
        {
            return value == Guid.Empty;
        }
    }
}
