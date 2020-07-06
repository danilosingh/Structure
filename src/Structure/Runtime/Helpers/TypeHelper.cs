using Structure.Extensions;
using Structure.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Structure.Helpers
{
    public static class TypeHelper
    {
        private static readonly Dictionary<Type, Dictionary<IMemberInfo, IMemberInfo>> cachePropWithBackfield;

        static TypeHelper()
        {
            cachePropWithBackfield = new Dictionary<Type, Dictionary<IMemberInfo, IMemberInfo>>();
        }

        public static object GetFieldValue(object obj, string name)
        {
            var field = obj.GetType().GetField(name, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (field == null)
            {
                return null;
            }

            return field.GetValue(obj);
        }

        public static T GetFieldValue<T>(object obj, string name)
        {
            object valor = GetFieldValue(obj, name);
            return valor != null ? (T)valor : default;
        }

        public static void SetFieldValue(object obj, string name, object value)
        {
            var field = obj.GetType().GetField(name, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (field == null)
            {
                throw new ArgumentException("Field não encontrado.");
            }

            field.SetValue(obj, value);
        }

        public static object GetPropertyValue(object obj, string name)
        {
            var prop = obj.GetType().GetProperties().FirstOrDefault(c => c.Name == name);

            if (prop == null)
            {
                return null;
            }

            return prop.GetValue(obj, null);
        }

        public static void SetPropertyValue(object obj, string name, object value)
        {
            PropertyInfo prop = GetPropertyInfo(obj.GetType(), name);

            if (prop == null) throw new ArgumentException("Property not found.");

            prop.SetValue(obj, value, null);
        }

        public static void SetPropertyValue(object obj, PropertyInfo prop, object value)
        {
            prop.SetValue(obj, value, null);
        }

        public static void TrimStringProperties(object obj)
        {
            var properties = TypeHelper.GetProperties(obj.GetType())
                .Where(c => typeof(string).IsAssignableFrom(c.PropertyType) && c.CanWrite);

            foreach (var item in properties)
            {
                var value = System.Convert.ToString(item.GetValue(obj, null));

                if (!string.IsNullOrEmpty(value))
                {
                    item.SetValue(obj, value.Trim(), null);
                }
            }
        }

        public static object GetNestedFieldValue(object obj, string name, BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance)
        {
            if (obj == null || obj is System.DBNull)
            {
                return null;
            }

            object value = obj;

            foreach (var prop in name.Split('.').Select(s => value.GetType().GetField(s, bindingFlags)))
            {
                value = prop.GetValue(value);

                if (value == null)
                {
                    return null;
                }
            }

            return value;
        }

        public static object GetNestedPropertyValue(object obj, string propertyName)
        {
            if (obj == null || obj is DBNull)
            {
                return null;
            }

            object value = obj;

            foreach (var prop in propertyName.Split('.').Select(s => value.GetType().GetProperty(s)))
            {
                if (prop == null)
                {
                    return null;
                }

                value = prop.GetValue(value, null);

                if (value == null)
                {
                    return null;
                }
            }

            return value;
        }

        public static IList<FieldInfo> GetFields(Type type)
        {
            IList<FieldInfo> fieldsInfo = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            Type baseType = type.BaseType;

            while (baseType != null && baseType.FullName != "System.Object")
            {
                fieldsInfo = fieldsInfo.Concat(baseType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).ToArray();
                baseType = baseType.BaseType;
            }

            return fieldsInfo.ToList();
        }

        public static IList<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
        }

        public static IList<PropertyInfo> GetPropertiesOf<T>(Type type)
        {
            return GetProperties(type).Where(c => c.PropertyType.InheritOrImplement<T>()).ToList();
        }

        public static IList<PropertyInfo> GetPropertiesWritable(Type type)
        {
            return GetProperties(type).Where(c => c.CanWrite).ToList();
        }

        public static Dictionary<IMemberInfo, IMemberInfo> GetPropertiesWithBackfields(Type type, bool writable = true)
        {
            if (!cachePropWithBackfield.TryGetValue(type, out Dictionary<IMemberInfo, IMemberInfo> dictionary))
            {
                dictionary = new Dictionary<IMemberInfo, IMemberInfo>();

                var properties = type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(c => writable ? c.CanWrite : true)
                    .ToList();

                foreach (var prop in properties)
                {
                    dictionary.Add(new PropertyMemberInfo(prop), new FieldMemberInfo(GetBackFieldFromPropertyName(type, prop.Name)));
                }

                cachePropWithBackfield.Add(type, dictionary);
            }

            return dictionary;
        }

        public static FieldInfo GetBackFieldFromPropertyName(Type type, string propertyName)
        {
            string fieldName = string.Format("<{0}>k__BackingField", propertyName);
            var field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (field == null)
            {
                Type baseType = type.BaseType;

                while (field == null && baseType != null && baseType.FullName != "System.Object")
                {
                    field = baseType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    baseType = baseType.BaseType;
                }
            }

            return field;
        }

        public static FieldInfo GetField(Type type, string fieldName)
        {
            FieldInfo[] fieldsInfo = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            Type baseType = type.BaseType;

            while (baseType != null && baseType.FullName != "System.Object")
            {
                fieldsInfo = fieldsInfo.Concat(baseType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).ToArray();
                baseType = baseType.BaseType;
            }

            return fieldsInfo.FirstOrDefault(c => c.Name == fieldName);
        }

        public static PropertyInfo GetPropertyInfo<T>(string propertyName)
        {
            return GetPropertyInfo(typeof(T), propertyName);
        }

        public static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            return type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public static PropertyInfo GetNestedPropertyInfo<T>(object source, Expression<Func<T, object>> propertyExpression)
        {
            return GetNestedProperty(source.GetType(), ExpressionHelper.GetNestledPropertyName(propertyExpression));
        }

        public static PropertyInfo GetNestedProperty(Type type, string propertyName)
        {
            PropertyInfo propInfo = null;

            foreach (var propertyNamePart in propertyName.Split('.'))
            {
                propInfo = type.GetProperty(propertyNamePart, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = propInfo.PropertyType;
            }

            return propInfo;
        }

        public static bool HasProperty(Type type, string propertyName, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            return type.GetProperties(bindingAttr).Any(c => c.Name == propertyName);
        }

        public static T TryConvert<T>(object value, CultureInfo cultureInfo = null)
        {
            try
            {
                return Convert<T>(value, cultureInfo);
            }
            catch
            {
                return default;
            }
        }

        public static T Convert<T>(object value, CultureInfo cultureInfo = null)
        {
            if (value is T)
            {
                return (T)value;
            }

            if (value == null || value is DBNull)
            {
                return default;
            }

            if (cultureInfo == null)
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }

            var toType = typeof(T);

            if (toType.IsGenericType &&
                toType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                toType = Nullable.GetUnderlyingType(toType); ;
            }

            if (value is string)
            {
                if (toType == typeof(Guid))
                {
                    return Convert<T>(new Guid(System.Convert.ToString(value)), cultureInfo);
                }
                if ((string)value == string.Empty && toType != typeof(string))
                {
                    return Convert<T>(null, cultureInfo);
                }
            }
            else
            {
                if (typeof(T) == typeof(string))
                {
                    return Convert<T>(System.Convert.ToString(value, cultureInfo), cultureInfo);
                }
            }

            if (toType.IsEnum && value != null)
            {
                return (T)Enum.Parse(toType, value.ToString());
            }
            else if (toType == typeof(TimeSpan))
            {
                var converter = TypeDescriptor.GetConverter(typeof(TimeSpan));
                return (T)converter.ConvertFrom(value);
            }

            bool canConvert = toType is IConvertible || (toType.IsValueType && !toType.IsEnum);

            if (canConvert)
            {
                return (T)System.Convert.ChangeType(value, toType, cultureInfo);
            }
            return (T)value;
        }

        public static bool IsIEnumerable(Type type)
        {
            return type.IsGenericType
                && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        public static Type GetIEnumerableImpl(Type type)
        {
            if (IsIEnumerable(type))
            {
                return type;
            }

            Type[] t = type.FindInterfaces((m, o) => IsIEnumerable(m), null);

            return t[0];
        }

        public static T GetAttribute<T>(this Type member)
           where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            return (T)attribute;
        }

        public static TAttribute GetAttributeOfMemberOrDeclaringTypeOrDefault<TAttribute>(MemberInfo memberInfo, TAttribute defaultValue = default, bool inherit = true)
            where TAttribute : class
        {
            return memberInfo.GetCustomAttributes(inherit).OfType<TAttribute>().FirstOrDefault()
                   ?? memberInfo.ReflectedType?.GetTypeInfo().GetCustomAttributes(inherit).OfType<TAttribute>().FirstOrDefault()
                   ?? defaultValue;
        }

        public static T GetAttributeFromMemberInfo<T>(MemberInfo member, bool isRequired = false)
           where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The {0} attribute must be defined on member {1}",
                        typeof(T).Name,
                        member.Name));
            }

            return (T)attribute;
        }

        public static List<object> GetAttributesOfMemberAndType(MemberInfo memberInfo, Type type)
        {
            var attributes = new List<object>();
            attributes.AddRange(GetAttributesOfMember(memberInfo));
            attributes.AddRange(GetAttributesOfType(type));
            return attributes;
        }

        public static List<object> GetAttributesOfMember(MemberInfo memberInfo, bool inherit = true)
        {
            return new List<object>(memberInfo.GetCustomAttributes(inherit));
        }

        public static List<T> GetAttributesOfMember<T>(MemberInfo memberInfo, bool inherit = true)
        {
            return GetAttributesOfMember(memberInfo, inherit).OfType<T>().ToList();
        }

        public static List<object> GetAttributesOfType(Type type, bool inherit = true)
        {
            return new List<object>(type.GetCustomAttributes(inherit));
        }

        public static List<T> GetAttributesOfType<T>(Type type, bool inherit = true)
        {
            return GetAttributesOfType(type, inherit).OfType<T>().ToList();
        }

        public static bool IsPrimitiveExtendedIncludingNullable(Type type, bool includeEnums = false)
        {
            if (IsPrimitiveExtended(type, includeEnums))
            {
                return true;
            }

            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return IsPrimitiveExtended(type.GenericTypeArguments[0], includeEnums);
            }

            return false;
        }

        private static bool IsPrimitiveExtended(Type type, bool includeEnums)
        {
            if (type.GetTypeInfo().IsPrimitive)
            {
                return true;
            }

            if (includeEnums && type.GetTypeInfo().IsEnum)
            {
                return true;
            }

            return type == typeof(string) ||
                   type == typeof(decimal) ||
                   type == typeof(DateTime) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(Guid);
        }
    }
}
