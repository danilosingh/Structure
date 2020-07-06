using Structure.Domain.Events;
using System.Collections.Generic;

namespace Structure.Domain.Entities
{
    public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot
    {
        private readonly List<IDomainEvent> domainEvents = new List<IDomainEvent>();
        public virtual IReadOnlyList<IDomainEvent> DomainEvents => domainEvents;

        protected virtual void AddDomainEvent(IDomainEvent @event)
        {
            domainEvents.Add(@event);
        }

        public virtual void ClearDomainEvents()
        {
            domainEvents.Clear();
        }
    }
}
