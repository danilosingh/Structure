using Structure.Infrastructure.Messaging.EventBus.Events;
using System.Collections.Generic;
using System.Linq;

namespace Structure.Messaging.EventBus.Abstractions
{
    public class InMemoryDistributedEventStore : IDistributedEventStore
    {
        private readonly List<IntegrationEvent> unpublishedEvents;
        private readonly IEventBus eventBus;

        public IReadOnlyCollection<IntegrationEvent> UnpublishedEvents { get { return unpublishedEvents.AsReadOnly(); } }

        public InMemoryDistributedEventStore(IEventBus eventBus)
        {
            unpublishedEvents = new List<IntegrationEvent>();
            this.eventBus = eventBus;
        }

        public void Add(IntegrationEvent @event)
        {
            unpublishedEvents.Add(@event);
        }

        public void PublishEvents()
        {
            foreach (var @event in unpublishedEvents.ToList())
            {
                eventBus.Publish(@event);
                unpublishedEvents.Remove(@event);
            }
        }
    }
}
