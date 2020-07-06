using System;

namespace Structure.Domain.Entities.Auditing
{
    public class AuditedAggregateRoot<T> : AggregateRoot<T>, IAudited
    {
        public virtual DateTime CreationTime { get; set; }
        public virtual Guid CreatorId { get; set; }
        public virtual Guid? LastModifierId { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
    }
}
