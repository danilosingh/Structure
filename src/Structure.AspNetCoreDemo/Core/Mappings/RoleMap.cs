using Structure.Nhibernate.Mapping;
using Structure.Tests.Shared.Entities;

namespace Structure.AspNetCoreDemo.Core.Mappings
{
    public class RoleMap : EntityClassMap<Role>
    {
        public RoleMap()
        {
            Id(c => c.Id).GeneratedBy.GuidComb();
            Map(c => c.Name).Indexable().Length(60);
            Map(c => c.NormalizedName).Length(60);
            //Component(c => c.RoleComponent, (d) =>
            //{
            //    d.References(e => e.User).Indexable();
            //    d.Map(e => e.Value).Indexable();
            //});
        }
    }
}
