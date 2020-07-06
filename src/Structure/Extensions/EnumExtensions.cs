using Structure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Structure.Extensions
{
    public static class EnumExtensions
    {
        public static string ToIntString(this Enum value)
        {
            return Convert.ToInt32(value).ToString();
        }

        public static string ToDisplayString(this Enum value)
        {
            return EnumHelper.GetDisplayName(value);
        }

        public static T ToFlag<T>(this IEnumerable<T> values)
            where T : Enum
        {
            int? result = null;

            foreach (var enumVal in values)
            {
                var intVal = Convert.ToInt32(enumVal);

                if (result.HasValue == false)
                    result = intVal;

                result |= intVal;
            }

            return (T)Enum.ToObject(typeof(T), result ?? 0);
        }

        public static IList<T> ToFlagList<T>(this T value)
           where T : Enum
        {
            return Enum.GetValues(value.GetType()).Cast<Enum>()
                .Where(value.HasFlag)
                .OfType<T>()
                .ToList();
        }
    }
}
