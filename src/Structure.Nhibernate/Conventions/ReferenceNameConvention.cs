using FluentNHibernate;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System;
using System.Linq;

namespace Structure.Nhibernate.Conventions
{
    public class ReferenceNameConvention : IReferenceConvention, IConvention<IManyToOneInspector, IManyToOneInstance>, IHasManyToManyConvention, IConvention<IManyToManyCollectionInspector, IManyToManyCollectionInstance>, IJoinedSubclassConvention, IConvention<IJoinedSubclassInspector, IJoinedSubclassInstance>, IJoinConvention, IConvention<IJoinInspector, IJoinInstance>, ICollectionConvention, IConvention<ICollectionInspector, ICollectionInstance>, IConvention
    {
        public void Apply(ICollectionInstance instance)
        {
            var relationshipType = instance.ChildType;
            var property = relationshipType.GetInstanceProperties().FirstOrDefault(c => c.PropertyType == instance.EntityType);
            instance.Key.Column(GetConventionname(property?.Name, instance.EntityType));
        }

        public void Apply(IJoinedSubclassInstance instance)
        {
            instance.Key.Column(GetConventionname(null, instance.Type.BaseType));
        }

        public void Apply(IJoinInstance instance)
        {
            instance.Key.Column(GetConventionname(null, instance.EntityType));
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            instance.Key.Column(GetConventionname(null, instance.EntityType));
            instance.Relationship.Column(GetConventionname(null, instance.ChildType));
        }

        public void Apply(IManyToOneInstance instance)
        {
            instance.Column(GetConventionname(instance.Property.Name, instance.Class.GetUnderlyingSystemType()));
        }

        private string GetConventionname(string propertyName, Type type)
        {
            return (propertyName ?? type.Name) + "Id".ToConventionCase();
        }
    }
}
