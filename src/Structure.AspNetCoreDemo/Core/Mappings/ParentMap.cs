using Structure.Nhibernate.Mapping;
using Structure.Tests.Shared.Domain.Entities;

namespace Structure.AspNetCoreDemo.Core.Mappings
{
    public class ParentMap : EntityClassMap<Parent>
    {
        public ParentMap()
        {
            
        }

        protected override void PerformMapping()
        {
            base.PerformMapping();
            Id(c => c.Id).GeneratedBy.Assigned();
            
        }
    }
}
