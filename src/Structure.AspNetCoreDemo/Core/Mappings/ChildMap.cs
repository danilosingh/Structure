using Structure.Nhibernate.Mapping;
using Structure.Tests.Shared.Domain.Entities;

namespace Structure.AspNetCoreDemo.Core.Mappings
{
    public class ChildMap : EntitySubclassMap<Child>
    {
        public ChildMap()
        {
        }

        protected override void PerformMapping()
        {
            base.PerformMapping();
            References(c => c.Role).Indexable();
            KeyColumn("PersonId");
            Table("Child_custom");
        }
    }
}
