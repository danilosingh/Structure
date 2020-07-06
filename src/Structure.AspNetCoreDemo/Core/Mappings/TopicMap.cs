using Structure.Nhibernate.Mapping;
using Structure.Tests.Shared.Entities;

namespace Structure.AspNetCoreDemo.Core.Mappings
{
    public class TopicMap : AuditedEntityClassMap<Topic>
    {
        protected override void PerformMapping()
        {
            Id(c => c.Id).GeneratedBy.GuidComb();
            Map(c => c.Name).Indexable().Length(100).Not.Nullable();
            References(c => c.ParentTopic).Indexable();
            //Map(c => c.Ordination,);
            Component(c => c.TopicComponent, map =>
            {
                map.Map(c => c.Value).Indexable();
                map.Component(c => c.Child, (d) =>
                {
                    d.Map(e => e.ValeChildComponent).Indexable();
                });
            });
        }
    }
}
