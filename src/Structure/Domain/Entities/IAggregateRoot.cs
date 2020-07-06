using Structure.Domain.Events;
using System.Collections.Generic;

namespace Structure.Domain.Entities
{
    public interface IAggregateRoot : IEntity
    {
        IReadOnlyList<IDomainEvent> DomainEvents { get; }
    }
}
