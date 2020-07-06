using Structure.Domain.Entities;
using Structure.Helpers;
using Structure.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Structure.Domain.Helpers
{
    public static class EntityClonerHelper
    {
        private static readonly List<Func<object, Type>> realTypeResolvers;
        private static readonly List<Func<object, string, bool>> checkersProxyProperties;

        static EntityClonerHelper()
        {
            realTypeResolvers = new List<Func<object, Type>>();
            checkersProxyProperties = new List<Func<object, string, bool>>();
        }

        private static Type GetRealType(object proxy)
        {
            if (realTypeResolvers != null && realTypeResolvers.Count > 0)
            {
                Type type = null;

                foreach (var item in realTypeResolvers)
                {
                    type = item(proxy);

                    if (type != null)
                    {
                        return type;
                    }
                }
            }

            return proxy.GetType();
        }

        public static void AddRealTypeResolver(Func<object, Type> resolver)
        {
            realTypeResolvers.Add(resolver);
        }

        public static void AddProxyCheckerProperties(Func<object, string, bool> checker)
        {
            checkersProxyProperties.Add(checker);
        }

        private static object CloningProcess(object source, Dictionary<object, object> circularReferences, int maxDeph)
        {
            if (circularReferences.ContainsKey(source))
            {
                return circularReferences[source];
            }

            Type type = GetRealType(source);

            object clonedObject = Activator.CreateInstance(type);

            if (clonedObject is IEntity)
            {
                var field = type.GetField("instaceId", BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (field != null)
                {
                    field.SetValue(clonedObject, (source as IEntity).InstaceId);
                }
            }

            circularReferences[source] = clonedObject;

            var membersInfo = TypeHelper.GetPropertiesWithBackfields(type);

            foreach (var item in membersInfo)
            {
                IMemberInfo member = item.Key;

                var objeto = member.GetValue(source);

                if (objeto != null && objeto is Entity)
                {
                    Entity objRef = objeto as Entity;
                    member.SetValue(clonedObject, maxDeph < 0 ? objRef : CloningProcess(objRef, circularReferences, maxDeph - 1));
                    continue;
                }
                else
                {
                    member.SetValue(clonedObject, objeto);
                }

                if (member.MemberType.FullName.ToLower().Equals("system.string") ||
                    member.MemberType.Name.Contains("Byte[]"))
                {
                    continue;
                }

                #region Clonagem de enumerables

                Type IListType = member.MemberType.GetInterface("IEnumerable", true);

                if (IListType != null)
                {
                    try
                    {
                        IEnumerable iListValue = (IEnumerable)member.GetValue(source);

                        if (iListValue == null)
                        {
                            continue;
                        }

                        Type genericTypeList = null;

                        if (iListValue.GetType().IsGenericType && iListValue.GetType().GetGenericArguments().Length > 0)
                        {
                            if (maxDeph < 0 && iListValue.GetType().GetGenericArguments().Any(c => typeof(IEntity).IsAssignableFrom(c)))
                            {
                                continue;
                            }

                            genericTypeList = typeof(List<>);
                            genericTypeList = genericTypeList.MakeGenericType(iListValue.GetType().GetGenericArguments());
                        }
                        else
                        {
                            genericTypeList = iListValue.GetType();
                        }

                        IList newList = (IList)Activator.CreateInstance(genericTypeList);

                        foreach (object value in iListValue)
                        {
                            if (value is Entity)
                            {
                                Entity clone = value as Entity;
                                newList.Add(CloningProcess(clone, circularReferences, maxDeph - 1));
                            }
                            else
                            {
                                newList.Add(value);
                            }
                        }

                        member.SetValue(clonedObject, newList);
                    }
                    catch
                    {
                        member.SetValue(clonedObject, null);
                    }
                }

                #endregion
            }

            return clonedObject;
        }

        public static object Clone(object source)
        {
            return CloningProcess(source, new Dictionary<object, object>(), 3);
        }

        public static object Clone(object source, int maxDeph)
        {
            return CloningProcess(source, new Dictionary<object, object>(), maxDeph);
        }
        
        public static T Clone<T>(object source)
        {
            return (T)Clone(source);
        }
    }
}
