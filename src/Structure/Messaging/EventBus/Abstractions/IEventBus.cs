using Structure.Infrastructure.Messaging.EventBus.Events;

namespace Structure.Messaging.EventBus.Abstractions
{
    public interface IEventBus
    {            
        void Publish(IntegrationEvent @event);

        void Subscribe<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>;

        void SubscribeDynamic<THandler>(string eventName)
            where THandler : IDynamicIntegrationEventHandler;

        void UnsubscribeDynamic<THandler>(string eventName)
            where THandler : IDynamicIntegrationEventHandler;

        void Unsubscribe<TEvent, THandler>()
            where THandler : IIntegrationEventHandler<TEvent>
            where TEvent : IntegrationEvent;

        void Dispose();
    }
}
