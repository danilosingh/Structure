using Structure.Nhibernate.Mapping;
using Structure.Tests.Shared.Entities;

namespace Structure.AspNetCoreDemo.Core.Mappings
{
    public class TopicChildMap : EntityClassMap<TopicChild>
    {
        public TopicChildMap()
        {            
            Id(c => c.Id).GeneratedBy.Native();
            Map(c => c.Name).Length(60);
            References(c => c.Another);
        }
    }
}
