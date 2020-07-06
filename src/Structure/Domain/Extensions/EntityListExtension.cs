using Structure.Collections;
using Structure.Collections.Extensions;
using Structure.Domain.Entities;
using Structure.Domain.Helpers;
using Structure.Extensions;
using Structure.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Structure.Domain.Extensions
{
    public static class EntityListExtension
    {
        private static readonly Dictionary<Type, IList<FieldInfo>> fieldsInfoCache = new Dictionary<Type, IList<FieldInfo>>();

        public static void Edit<TEntity>(this IList<TEntity> list, int index, TEntity editedItem)
        {
            Edit(list as IList, index, editedItem);
        }

        public static bool Edit<TEntity>(this IList<TEntity> list, Predicate<TEntity> predicate, TEntity editedItem)
        {
            int index = list.IndexOf(predicate);

            if (index < 0)
            {
                return false;
            }

            (list as IList).Edit(index, editedItem);

            return true;
        }

        public static bool Remove<TEntity>(this IList<TEntity> list, Predicate<TEntity> predicate)
        {
            int index = list.IndexOf(predicate);

            if (index < 0)
            {
                return false;
            }

            list.RemoveAt(index);

            return true;
        }

        public static void RemoveAll<TEntity>(this IList<TEntity> list, Predicate<TEntity> predicate)
        {
            int index = list.IndexOf(predicate);

            while (index >= 0)
            {
                list.RemoveAt(index);
                index = list.IndexOf(predicate);
            }
        }

        public static bool RemoveById<TEntity>(this IList<TEntity> list, TEntity entity)
            where TEntity : IEntity
        {
            int index = list.IndexOf(c => EntityHelper.Equals(entity, c));

            if (index < 0)
            {
                return false;
            }

            list.RemoveAt(index);

            return true;
        }

        public static bool AddOrEdit<TEntity>(this IList<TEntity> list, Predicate<TEntity> predicate, TEntity editedItem)
        {
            int index = list.IndexOf(predicate);

            if (index < 0)
            {
                list.Add(editedItem);
            }
            else
            {
                list.Edit(index, editedItem);
            }

            return true;
        }

        public static bool AddIfNotExists<TEntity>(this IList<TEntity> list, Predicate<TEntity> predicate, TEntity editedItem)
        {
            int index = list.IndexOf(predicate);

            if (index < 0)
            {
                list.Add(editedItem);
                return true;
            }

            return false;
        }

        public static bool AddOrEditById<TEntity, TId>(this IList<TEntity> list, TEntity editedItem)
            where TEntity : IEntity<TId>
        {
            int index = list.IndexOf(c => EntityHelper.Equals(editedItem, c));

            if (index >= 0)
            {
                list.Edit(index, editedItem);
            }
            else
            {
                list.Add(editedItem);
            }

            return true;
        }

        public static bool EditById<TEntity, TId>(this IList<TEntity> list, TEntity editedItem)
            where TEntity : IEntity<TId>
        {
            int index = list.IndexOf(c => EntityHelper.Equals(editedItem, c));

            if (index >= 0)
            {
                list.Edit(index, editedItem);
                return true;
            };

            return false;
        }

        public static bool Exists<T>(this IList<T> list, Predicate<T> predicate)
        {
            return list.IndexOf(predicate) >= 0;
        }

        public static bool Exists<TEntity>(this IList<TEntity> list, TEntity entity)
            where TEntity : IEntity
        {
            return list.IndexOf(c => EntityHelper.Equals(c, entity)) >= 0;
        }

        public static int IndexOf<TEntity, TId>(this IList<TEntity> list, TEntity entity)
            where TEntity : IEntity<TId>
        {
            return list.IndexOf(c => EntityHelper.Equals(entity, c));
        }

        #region Private methods

        private static int AddOrEditList<TEntity>(this IList list, TEntity editedItem)
            where TEntity : IEntity
        {
            int index = list.IndexOf<IEntity>(c => EntityHelper.Equals(editedItem, c));

            if (index >= 0)
            {
                list.Edit(index, editedItem);
                return index;
            }
            else
            {
                return list.Add(editedItem);
            }
        }

        private static void Edit(this IList list, int index, object editedItem)
        {
            IList<FieldInfo> fields = GetFieldTypes(editedItem.GetType());

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(editedItem);

                if (value != null && value.GetType().IsGenericType &&
                    value.GetType().InheritOrImplement(typeof(IEnumerable)) &&
                    value.GetType().InheritOrImplement(typeof(IEntity)))
                {
                    var newCastedList = ((IEnumerable)value).Cast<IEntity>();
                    var currentList = field.GetValue(list[index]) as IList;

                    IList<int> changedIndexes = new List<int>();

                    foreach (var newItem in newCastedList)
                    {
                        changedIndexes.Add(currentList.AddOrEditList(newItem));
                    }

                    for (int j = 0; j < currentList.Count; j++)
                    {
                        if (!changedIndexes.Any(c => c == j))
                        {
                            currentList.RemoveAt(j);
                        }
                    }
                }
                else
                {
                    field.SetValue(list[index], value);
                }
            }
        }

        private static IList<FieldInfo> GetFieldTypes(Type type)
        {
            fieldsInfoCache.TryGetValue(type, out IList<FieldInfo> fields);

            if (fields != null)
            {
                return fields;
            }

            fields = TypeHelper.GetFields(type);

            int i = fields.IndexOf(c => c.Name == "_guid" && c.DeclaringType.Name.Contains("Entity"));

            if (i > 0)
            {
                fields.RemoveAt(i);
            }

            i = fields.IndexOf(c => c.Name.Contains("<Id>") && c.DeclaringType.Name.Contains("Entity"));

            if (i > 0)
            {
                fields.RemoveAt(i);
            }

            fieldsInfoCache.Add(type, fields);

            return fields;
        }

        #endregion
    }
}
