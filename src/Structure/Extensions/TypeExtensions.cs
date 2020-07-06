using Structure.Helpers;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Structure.Extensions
{
    public static class TypeExtensions
    {
        private static readonly ConcurrentDictionary<string, bool> inheritOrImplementCache = new ConcurrentDictionary<string, bool>();

        public static bool InheritOrImplement(this Type childType, Type parentType, bool checkGenericArgument = true)
        {
            if (childType == parentType)
            {
                return true;
            }

            parentType = ResolveGenericTypeDefinition(parentType);

            string key = childType.FullName + parentType.FullName;

            if (inheritOrImplementCache.ContainsKey(childType.FullName + parentType.FullName))
            {
                return inheritOrImplementCache[key];
            }

            if (childType.IsGenericType && checkGenericArgument)
            {
                var arguments = childType.GetGenericArguments();

                if (arguments != null && arguments.Count() > 0)
                {
                    var arg = arguments[0];

                    if (arg.InheritOrImplement(parentType))
                    {
                        return ReturnInheritOrImplement(parentType, childType, true);
                    }
                }
            }

            var currentChildType = childType.IsGenericType
                                   ? childType.GetGenericTypeDefinition()
                                   : childType;

            while (currentChildType != typeof(object))
            {
                if (currentChildType.IsAssignableFrom(parentType) || parentType == currentChildType || HasAnyInterfaces(parentType, currentChildType))
                {
                    return ReturnInheritOrImplement(parentType, childType, true);
                }

                currentChildType = currentChildType.BaseType != null && currentChildType.BaseType.IsGenericType ?
                    currentChildType.BaseType.GetGenericTypeDefinition() : currentChildType.BaseType;

                if (currentChildType == null)
                {
                    return ReturnInheritOrImplement(parentType, childType, false);
                }
            }

            return ReturnInheritOrImplement(parentType, childType, false);
        }

        private static bool ReturnInheritOrImplement(Type parentType, Type childType, bool value)
        {
            if (!inheritOrImplementCache.ContainsKey(parentType.FullName + childType.FullName))
            {
                inheritOrImplementCache.AddOrUpdate(parentType.FullName + childType.FullName, value, (key, v) => value);
            }

            return value;
        }

        public static bool InheritOrImplement<T>(this Type childType)
        {
            return childType.InheritOrImplement(typeof(T));
        }

        public static bool InheritOrImplement(this Type childType, params Type[] parentTypes)
        {
            foreach (var item in parentTypes)
            {
                if (childType.InheritOrImplement(item, false))
                {
                    return true;
                }
            }

            return false;
        }

        public static string GetGenericTypeName(this Type type)
        {
            var typeName = string.Empty;

            if (type.IsGenericType)
            {
                var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                typeName = type.Name;
            }

            return typeName;
        }

        public static T ChangeType<T>(this object value, CultureInfo cultureInfo)
        {
            return TypeHelper.Convert<T>(value, cultureInfo);
        }

        public static T ChangeType<T>(this object value)
        {
            return TypeHelper.Convert<T>(value);
        }

        public static object GetDefaultValue(this Type t)
        {
            if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
                return Activator.CreateInstance(t);
            else
                return null;
        }

        public static bool HasProperty(this Type type, string propertyName, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
        {
            return type.GetProperties(bindingAttr).Any(c => c.Name == propertyName);
        }

        private static bool HasAnyInterfaces(Type parent, Type child)
        {
            return child.GetInterfaces()
                .Any(childInterface =>
                {
                    var currentInterface = childInterface.IsGenericType
                        ? childInterface.GetGenericTypeDefinition()
                        : childInterface;

                    return currentInterface == parent;
                });
        }

        private static Type ResolveGenericTypeDefinition(Type parent)
        {
            var shouldUseGenericType = true;

            if (parent.IsGenericType && parent.GetGenericTypeDefinition() != parent)
                shouldUseGenericType = false;

            if (parent.IsGenericType && shouldUseGenericType)
                parent = parent.GetGenericTypeDefinition();

            return parent;
        }

        public static bool In(this Type type, params Type[] types)
        {
            foreach (var item in types)
            {
                if (type == item)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool In<T1, T2>(this Type type)
        {
            return type.In(typeof(T1), typeof(T2));
        }

        public static bool In<T1, T2, T3>(this Type type)
        {
            return type.In(typeof(T1), typeof(T2), typeof(T3));
        }

        public static object GetDefault(this Type type)
        {
            if (type == null || !type.IsValueType || type == typeof(void))
                return null;

            if (type.ContainsGenericParameters)
                throw new ArgumentException("{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
                    "> contains generic parameters, so the default value cannot be retrieved");

            if (type.IsPrimitive || !type.IsNotPublic)
            {
                try
                {
                    return Activator.CreateInstance(type);
                }
                catch (Exception e)
                {
                    throw new ArgumentException(
                        $"{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe Activator.CreateInstance method could not " +
                        "create a default instance of the supplied value type <" + type +
                        "> (Inner Exception message: \"" + e.Message + "\")", e);
                }
            }

            throw new ArgumentException("{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
                "> is not a publicly-visible type, so the default value cannot be retrieved");
        }

        public static Type MakeGenericTypeNotNullArguments(this Type type, params Type[] types)
        {
            return type.MakeGenericType(types.Where(c => c != null).ToArray());
        }
    }
}
