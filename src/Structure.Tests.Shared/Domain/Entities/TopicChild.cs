using Structure.Domain;
using Structure.Domain.Entities;

namespace Structure.Tests.Shared.Entities
{
    public class TopicChild : Entity<int>
    {
        public virtual Topic Another { get; set; }
        public virtual string Name { get; set; }
    }
}