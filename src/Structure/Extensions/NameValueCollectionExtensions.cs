using Structure.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Linq;

namespace Structure.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static string String(this NameValueCollection collection, string key)
        {
            return collection[key];
        }

        public static bool String(this NameValueCollection collection, string key, out string value)
        {
            var containsKey = collection.ContainsKey(key);
            value = containsKey ? collection.String(key) : default(string);
            return containsKey;
        }

        public static bool String(this NameValueCollection collection, string[] keys, out string value)
        {
            value = null;

            foreach (var key in keys)
            {
                if (collection.String(key, out value))
                {
                    return true;
                }
            }

            return false;
        }

        public static DateTime Date(this NameValueCollection collection, string key)
        {
            return TypeHelper.Convert<DateTime>(collection[key]);
        }

        public static DateTime? NullableDate(this NameValueCollection collection, string key)
        {
            return TypeHelper.Convert<DateTime?>(collection[key]);
        }

        public static bool Date(this NameValueCollection collection, string key, out DateTime value, DateTime? minValue)
        {
            var containsKey = collection.ContainsKey(key);
            value = containsKey ? collection.Date(key) : default(DateTime);
            return containsKey && (minValue == null || value > minValue.Value);
        }

        public static bool SqlDate(this NameValueCollection collection, string key, out DateTime value)
        {
            return collection.Date(key, out value, SqlDateTime.MinValue.Value);
        }

        public static Guid Guid(this NameValueCollection collection, string key)
        {
            return TypeHelper.Convert<Guid>(collection[key]);
        }

        public static bool ArrayEnum<T>(this NameValueCollection collection, string key, out IEnumerable<T> value)
            where T : struct
        {
            var containsKey = collection.ContainsKey(key);
            value = containsKey ? collection.ArrayEnum<T>(key) : null;
            return containsKey;
        }

        public static IEnumerable<T> ArrayEnum<T>(this NameValueCollection collection, string key)
            where T : struct
        {
            var containsKey = collection.ContainsKey(key);

            if (!containsKey)
            {
                return new List<T>();
            }

            try
            {
                return collection[key].Split(",").Select(c => EnumHelper.TryPase<T>(c));
            }
            catch
            {
                return new List<T>();
            }
        }

        public static bool Guid(this NameValueCollection collection, string key, out Guid value)
        {
            var containsKey = collection.ContainsKey(key);
            value = containsKey ? collection.Guid(key) : default(Guid);
            return containsKey;
        }

        public static decimal Decimal(this NameValueCollection collection, string key)
        {
            return TypeHelper.TryConvert<decimal>(collection[key]);
        }

        public static bool Decimal(this NameValueCollection collection, string key, out decimal value)
        {
            var containsKey = collection.ContainsKey(key);
            value = containsKey ? collection.Decimal(key) : default(decimal);
            return containsKey;
        }

        public static int Integer(this NameValueCollection collection, string key)
        {
            return TypeHelper.TryConvert<int>(collection[key]);
        }

        public static bool Integer(this NameValueCollection collection, string key, out int value)
        {
            var containsKey = collection.ContainsKey(key);
            value = containsKey ? collection.Integer(key) : default(int);
            return containsKey;
        }

        public static bool ContainsKey(this NameValueCollection @this, string key)
        {
            return @this.Get(key) != null || @this.Keys.Cast<string>().Contains(key, StringComparer.OrdinalIgnoreCase);
        }
    }
}
