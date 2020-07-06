using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Structure.Infrastructure.Messaging.EventBus.Events;
using System;

namespace Structure.Messaging.EventBus.Abstractions
{
    public abstract class AbstractEventBus : IEventBus
    {
        protected readonly IServiceProvider serviceProvider;
        protected readonly IEventBusSubscriptionsManager subscriptionsManager;
        protected readonly ILogger<IEventBus> logger;

        protected AbstractEventBus(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.logger = serviceProvider.GetService<ILogger<IEventBus>>();
            this.subscriptionsManager = serviceProvider.GetService<IEventBusSubscriptionsManager>();
        }

        public void Dispose()
        {
            Dispose(true);            
            subscriptionsManager.Clear();
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        { }

        public abstract void Publish(IntegrationEvent @event);

        public void Subscribe<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            subscriptionsManager.AddSubscription<TEvent, THandler>();
        }

        public void SubscribeDynamic<THandler>(string eventName) where THandler : IDynamicIntegrationEventHandler
        {
            subscriptionsManager.AddDynamicSubscription<THandler>(eventName);
        }

        public void Unsubscribe<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            subscriptionsManager.RemoveSubscription<TEvent, THandler>();
        }

        public void UnsubscribeDynamic<THandler>(string eventName) where THandler : IDynamicIntegrationEventHandler
        {
            subscriptionsManager.RemoveDynamicSubscription<THandler>(eventName);
        }
    }
}
