using Structure.Infrastructure.Messaging.EventBus.Events;
using System;

namespace Structure.AspNetCoreDemo.Events.Integration
{
    public class UserRegistered : IntegrationEvent
    {
        public Guid UserId { get; set; }
    }
}
