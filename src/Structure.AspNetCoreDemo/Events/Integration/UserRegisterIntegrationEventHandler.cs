using Structure.Messaging.EventBus.Abstractions;
using System;
using System.Threading.Tasks;

namespace Structure.AspNetCoreDemo.Events.Integration
{
    public class UserRegisteredIntegrationEventHandler : IIntegrationEventHandler<UserRegistered>
    {
        public Task Handle(UserRegistered @event)
        {
            Console.WriteLine("ok");
            return Task.CompletedTask;
        }
    }
}
