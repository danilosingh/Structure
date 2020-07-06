using Structure.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace Structure.Tests.Shared.Entities
{
    public class Topic : AuditedEntity<Guid>
    {
        public virtual string Name { get; set; }
        public virtual User User { get; set; }
        public virtual Topic ParentTopic { get; set; }
        public virtual IList<TopicChild> Children { get; set; }
        public virtual int Ordination { get; set; }
        public virtual TopicComponent TopicComponent { get; set; }
        public Topic()
        {
            Children = new List<TopicChild>();
        }
    }
}
