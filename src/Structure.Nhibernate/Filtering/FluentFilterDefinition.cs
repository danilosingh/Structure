using FluentNHibernate.Mapping;
using Structure.Data.Filtering;
using Structure.Extensions;
using System.Linq;

namespace Structure.Nhibernate.Filtering
{
    internal class FluentFilterDefinition<TDataFilter> : FilterDefinition
        where TDataFilter : IDataFilter
    {
        public FluentFilterDefinition()
        {
            var nhDataFilter = FluentSessionMappingConfig.Instance.Filters
                .SingleOrDefault(c => c.Is(typeof(TDataFilter)));

            WithName(nhDataFilter.Name)
                .WithCondition(nhDataFilter.FilterDefinition.Condition);

            foreach (var item in nhDataFilter.FilterDefinition.Parameters)
            {
                AddParameter(item.Key.Remove(":"), item.Value.Type);
            }
        }
    }
}
