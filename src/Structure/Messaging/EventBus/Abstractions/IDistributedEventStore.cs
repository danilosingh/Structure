using Structure.Infrastructure.Messaging.EventBus.Events;
using System.Collections.Generic;

namespace Structure.Messaging.EventBus.Abstractions
{
    public interface IDistributedEventStore
    {
        IReadOnlyCollection<IntegrationEvent> UnpublishedEvents { get; }

        void Add(IntegrationEvent @event);
        void PublishEvents();
    }
}
