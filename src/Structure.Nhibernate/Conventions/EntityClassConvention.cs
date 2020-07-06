using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using Structure.Extensions;
using Structure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Structure.Nhibernate.Conventions
{
    public class EntityClassConvention : IClassConvention, ISubclassConvention, IJoinedSubclassConvention, IComponentConvention, IHasManyToManyConvention, ICompositeIdentityConvention
    {
        private readonly Dictionary<Type, IClassInstance> entities;

        public EntityClassConvention()
        {
            entities = new Dictionary<Type, IClassInstance>();
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            instance.Key.ForeignKey($"Fk_{instance.EntityType.Name}{instance.Member.Name}_{GetForeignKeyName(instance.Key.Columns)}".ToConventionCase());
            instance.Relationship.ForeignKey($"Fk_{instance.EntityType.Name}{instance.Member.Name}_{GetForeignKeyName(instance.Relationship.Columns)}".ToConventionCase());
        }

        public void Apply(IComponentInstance instance)
        {
            foreach (var item in instance.Properties)
            {
                item.Column(instance.Property.Name + item.Property.Name);
            }

            var entityInstance = entities.GetOrDefault(instance.EntityType);
            ApplyConventionForReferences(instance, entityInstance);
            ApplyConventionForProperties(instance, entityInstance);
        }

        public void Apply(IClassInstance instance)
        {
            entities.Add(instance.EntityType, instance);
            ApplyConventionForReferences(instance);
            ApplyConventionForProperties(instance);
            ApplyConventionForSubclasses(instance);
        }

        private void ApplyConventionForSubclasses(IClassInstance instance)
        {
            foreach (var subClassInspector in instance.Subclasses)
            {
                ApplyConventionForSubclasses(instance, subClassInspector);
            }
        }

        private void ApplyConventionForSubclasses(IInspector parentInpector, ISubclassInspectorBase subClassInspector)
        {
            ApplyConventionForReferences(subClassInspector, parentInpector);
            ApplyConventionForProperties(subClassInspector, parentInpector);

            foreach (var item in subClassInspector.Subclasses)
            {
                ApplyConventionForSubclasses(parentInpector, item);
            }
        }

        public void Apply(ISubclassInstance instance)
        {
            //ApplyConventionForReferences(instance);
            //ApplyConventionForProperties(instance);
        }

        public void Apply(IJoinedSubclassInstance instance)
        {
            ApplyConventionForReferences(instance);
            ApplyConventionForProperties(instance);
            instance.Key.ForeignKey($"Fk_{instance.EntityType.Name}_{GetForeignKeyName(instance.Key.Columns)}".ToConventionCase());
        }

        public void Apply(ICompositeIdentityInstance instance)
        {
            foreach (var item in instance.KeyManyToOnes)
            {
                var column = item.Columns.FirstOrDefault();
                item.ForeignKey($"Fk_{instance.EntityType.Name}_{(column != null ? column.Name.RemoveFromEnd("Id") : string.Empty)}".ToConventionCase());
            }
        }

        private void ApplyConventionForProperties(IInspector instance, IInspector parentInstance = null)
        {
            if (!(TypeHelper.GetPropertyValue(instance, "Properties") is IEnumerable<IPropertyInspector> properties))
            {
                return;
            }

            foreach (var property in properties)
            {
                property.Index(instance, property.Name, parentInstance);
            }
        }

        private void ApplyConventionForReferences(IInspector instance, IInspector parentInstance = null)
        {
            if (!(TypeHelper.GetPropertyValue(instance, "References") is IEnumerable<IManyToOneInspector> references))
            {
                return;
            }

            foreach (var manyToOneInspector in references)
            {
                manyToOneInspector.ForeignKey($"Fk_{instance.EntityType.Name }_{GetForeignKeyName(manyToOneInspector.Columns)}".ToConventionCase());
                manyToOneInspector.Index(instance, GetColumnName(manyToOneInspector.Columns), parentInstance);
            }
        }

        private string GetForeignKeyName(IEnumerable<IColumnInspector> columns)
        {
            return columns.FirstOrDefault().Name.RemoveFromEnd("Id", "_id");
        }

        private string GetColumnName(IEnumerable<IColumnInspector> columns)
        {
            return columns.FirstOrDefault().Name;
        }
    }

    public static class InspectorExtensions
    {
        static readonly Dictionary<Type, FieldInfo> mappingFieldsInfo;

        static InspectorExtensions()
        {
            mappingFieldsInfo = new Dictionary<Type, FieldInfo>();
        }

        public static ManyToOneMapping GetMapping(this IManyToOneInspector manyToOneInspector)
        {
            Type type = manyToOneInspector.GetType();
            mappingFieldsInfo.TryGetValue(type, out FieldInfo fieldInfo);

            if (fieldInfo == null)
            {
                fieldInfo = manyToOneInspector.GetType().GetField("mapping", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            if (fieldInfo != null)
            {
                var manyToOneMapping = fieldInfo.GetValue(manyToOneInspector) as ManyToOneMapping;
                return manyToOneMapping;
            }

            return null;
        }

        public static void ForeignKey(this IManyToOneInspector manyToOneInspector, string foreignKeyName)
        {
            var mapping = manyToOneInspector.GetMapping();
            mapping.ForeignKey(foreignKeyName);
        }

        public static void Index(this IInspector inspector, IInspector parentInspector, string memberName, IInspector grandParentInspector = null)
        {
            inspector.SetColumnsAttribute("Index", Layer.UserSupplied,
                () => $"Idx_{grandParentInspector?.GetInstanceName() ?? parentInspector.GetInstanceName()}_{memberName}".ToConventionCase(),
                (c) => c.Index == "___Indexable");
        }

        public static string GetInstanceName(this IInspector instance)
        {
            if (instance is IClassInstance classConvention)
            {
                return classConvention.TableName;
            }

            return instance.EntityType.Name;
        }

        public static void SetColumnsAttribute(this IInspector inspector, string attributeName, int layer,
            Func<object> getValue,
            Predicate<IColumnInspector> condition = null)
        {
            var columns = TypeHelper.GetPropertyValue(inspector, "Columns") as IEnumerable<IColumnInspector>;

            foreach (var column in columns)
            {
                if (!(condition?.Invoke(column) ?? true))
                {
                    continue;
                }

                column.SetAttribute(attributeName, getValue(), layer);
            }
        }


        public static void SetAttribute(this IColumnInspector columnInspector, string attributeName, object value, int layer)
        {
            var layeredValues = TypeHelper.GetNestedFieldValue(columnInspector, "mapping.attributes.layeredValues.inner") as Dictionary<string, LayeredValues>;

            if (layeredValues.ContainsKey(attributeName))
            {
                layeredValues[attributeName][layer] = value;
            }
            else
            {
                layeredValues.Add(attributeName, new LayeredValues() { { layer, value } });
            }
        }
    }

    public static class ManyToOneMappingExtensions
    {
        public static void ForeignKey(this ManyToOneMapping manyToOneMapping, string foreignKeyName)
        {
            if (!manyToOneMapping.IsSpecified("ForeignKey"))
            {
                manyToOneMapping.Set<string>(c => c.ForeignKey, 1, foreignKeyName);
            }
        }

        public static void ForeignKey(this KeyManyToOneMapping keyManyToOneMapping, string foreignKeyName)
        {
            if (!keyManyToOneMapping.IsSpecified("ForeignKey"))
            {
                keyManyToOneMapping.Set<string>(c => c.ForeignKey, 1, foreignKeyName);
            }
        }
    }
}
