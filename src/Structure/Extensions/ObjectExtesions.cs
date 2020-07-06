using Structure.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Structure.Extensions
{
    public static class ObjectExtesions
    {
        public static bool In<T>(this T @value, params T[] values)
        {
            return values.Contains(@value);
        }

        public static bool In<T>(this T @value, IQueryable<T> values)
        {
            return values.Contains(@value);
        }

        public static bool NotIn<T>(this T @value, params T[] values)
        {
            return !values.Contains(@value);
        }

        public static bool NotIn<T>(this T @value, IQueryable<T> values)
        {
            return !values.Contains(@value);
        }

        public static bool IsDefault<T>(this T @object)
        {
            return EqualityComparer<T>.Default.Equals(@object, default);
        }

        public static T GetPropertyValue<T>(this object @object, string propertyName)
        {
            return (T)TypeHelper.GetPropertyValue(@object, propertyName);
        }
    }
}
