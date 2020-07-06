using System;

namespace Structure.Domain.Entities.Auditing
{
    public abstract class CreationAuditedEntity<TId> : Entity<TId>, ICreationAudited
    {
        public virtual DateTime CreationTime { get; set; }

        public virtual Guid CreatorId { get; set; }

        protected CreationAuditedEntity()
        { }
    }
}