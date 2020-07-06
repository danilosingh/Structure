using System;

namespace Structure.Infrastructure.Messaging.EventBus.Events
{
    public class IntegrationEvent
    {
        public Guid Id  { get; }
        public DateTime CreationDate { get; }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }
    }
}
