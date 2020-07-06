using Structure.Domain.Entities;
using Structure.Domain.Entities.Auditing;
using Structure.Domain.Events;
using Structure.Identity;
using System;
using System.Collections.Generic;

namespace Structure.Tests.Shared.Entities
{
    public class User : IdentityUser, IAudited, IMultiTenant, ISoftDelete, IAggregateRoot
    {
        private readonly List<IDomainEvent> domainEvents;

        public override string UserName
        {
            get { return Email; }
            set { }
        }

        public virtual Guid CreatorId { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual Guid? LastModifierId { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual Guid? TenantId { get; set; }
        public virtual bool IsDeleted { get; set; }

        public virtual IReadOnlyList<IDomainEvent> DomainEvents
        {
            get
            {
                return domainEvents.AsReadOnly();
            }
        }

        public User()
        {
            domainEvents = new List<IDomainEvent>();
        }

        public virtual void Register(string namev)
        {
            domainEvents.Add(new UserRegisterDomainEvent() { User = this });
        }
    }
}
