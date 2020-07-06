using System;

namespace Structure.Domain.Entities.Auditing
{
    public abstract class AuditedEntity<TId> : CreationAuditedEntity<TId>, IAudited
    {
        public virtual DateTime? LastModificationTime { get; set; }

        public virtual Guid? LastModifierId { get; set; }
    }
}