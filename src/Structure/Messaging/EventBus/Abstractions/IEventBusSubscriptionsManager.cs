using Structure.Infrastructure.Messaging.EventBus.Events;
using System;
using System.Collections.Generic;
using static Structure.Infrastructure.Messaging.EventBus.InMemoryEventBusSubscriptionsManager;

namespace Structure.Messaging.EventBus.Abstractions
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }
        event EventHandler<string> OnEventRemoved;
        event EventHandler<string> OnAddSubscription;
        event EventHandler<string> OnRemoveSubscription;
        void AddDynamicSubscription<THandler>(string eventName)
           where THandler : IDynamicIntegrationEventHandler;

        void AddSubscription<TEvent, THandler>()
           where TEvent : IntegrationEvent
           where THandler : IIntegrationEventHandler<TEvent>;

        void RemoveSubscription<TEvent, THandler>()
             where THandler : IIntegrationEventHandler<TEvent>
             where TEvent : IntegrationEvent;
        void RemoveDynamicSubscription<THandler>(string eventName)
            where THandler : IDynamicIntegrationEventHandler;
        bool HasSubscriptionsForEvent<TEvent>() where TEvent : IntegrationEvent;
        bool HasSubscriptionsForEvent(string eventName);
        Type GetEventTypeByName(string eventName);
        void Clear();
        IEnumerable<SubscriptionInfo> GetHandlersForEvent<TEvent>() where TEvent : IntegrationEvent;
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);
        string GetEventKey<TEvent>();
    }
}