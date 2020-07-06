using FluentNHibernate.Mapping;

namespace Structure.Nhibernate.Mapping
{
    public abstract class EntityClassMap<T> : ClassMap<T>
    {
        protected EntityClassMap()
        {
            PerformMapping();
            ConfigureFilters();
        }

        protected virtual void ConfigureFilters()
        {
            foreach (var filter in FluentSessionMappingConfig.Instance.Filters)
            {
                if (!filter.IsEnabledForType(typeof(T)))
                {
                    continue;
                }

                ApplyFilter(filter.Name);
            }
        }

        protected virtual void PerformMapping()
        { }
    }
}
