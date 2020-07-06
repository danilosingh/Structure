using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Structure.Helpers
{
    public class EnumHelper
    {
        public static TEnum Parse<TEnum>(string strEnum) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), strEnum);
        }

        public static TEnum ToObject<TEnum>(object obj) where TEnum : struct
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), obj);
        }

        public static TEnum? ParseNullable<TEnum>(string strEnum) where TEnum : struct
        {
            TEnum saida;
            return Enum.TryParse<TEnum>(strEnum, out saida) ? saida : (Nullable<TEnum>)null;
        }

        public static TEnum TryPase<TEnum>(string strEnum) where TEnum : struct
        {
            TEnum saida;
            Enum.TryParse<TEnum>(strEnum, out saida);
            return saida;
        }

        public static TAttr GetAttribute<TAttr>(object value) where TAttr : System.Attribute
        {
            var type = value.GetType();
            var memInfo = type.GetMember(value.ToString());

            if (memInfo.Length == 0)
            {
                return null;
            }

            var attributes = memInfo[0].GetCustomAttributes(typeof(TAttr), false);

            return (attributes.Length > 0) ? (TAttr)attributes[0] : null;
        }

        public static string GetDisplayName(object value)
        {
            var attr = GetAttribute<DisplayAttribute>(value);

            if (attr != null)
            {
                return attr.Name;
            }

            return value.ToString();
        }

        public static bool IsDefaultValue<TEnum>(TEnum value)
        {
            return value.Equals(default(TEnum));
        }

        public static int GetIndex<TEnum>(TEnum value)
        {
            int i = -1;

            foreach (var item in Enum.GetValues(typeof(TEnum)))
            {
                i++;

                if (EqualityComparer<TEnum>.Default.Equals((TEnum)item, value))
                {
                    break;
                }
            }

            return i;
        }

        public static IEnumerable<TEnum> GetValues<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }

        public static IEnumerable<string> GetStringValues<TEnum>()
        {
            return GetValues<TEnum>().Select(c => Convert.ToString(c));
        }

        public static IEnumerable<string> GetIntStringValues<TEnum>()
        {
            return GetValues<TEnum>().Select(c => Convert.ToString(Convert.ToInt32(c)));
        }

        public static IList<TEnum> GetBitwise<TEnum>(TEnum value)
        {
            List<TEnum> list = new List<TEnum>();

            foreach (var item in Enum.GetValues(typeof(TEnum)))
            {
                if ((value as Enum).HasFlag((Enum)item))
                {
                    list.Add((TEnum)item);
                }
            }

            return list;
        }
    }
}
