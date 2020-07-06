using FluentNHibernate.Mapping;

namespace Structure.Nhibernate.Mapping
{
    public class EntitySubclassMap<T> : SubclassMap<T>
    {
        public EntitySubclassMap()
        {       
            PerformMapping();
        }

        protected virtual void PerformMapping()
        { }
    }
}
