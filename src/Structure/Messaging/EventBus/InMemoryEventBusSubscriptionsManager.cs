using Structure.Infrastructure.Messaging.EventBus.Events;
using Structure.Messaging.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Structure.Infrastructure.Messaging.EventBus
{
    public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> handlers;
        private readonly List<Type> eventTypes;

        public event EventHandler<string> OnEventRemoved;
        public event EventHandler<string> OnAddSubscription;
        public event EventHandler<string> OnRemoveSubscription;

        public InMemoryEventBusSubscriptionsManager()
        {
            handlers = new Dictionary<string, List<SubscriptionInfo>>();
            eventTypes = new List<Type>();
        }

        public bool IsEmpty => !handlers.Keys.Any();
        public void Clear() => handlers.Clear();

        public void AddDynamicSubscription<THandler>(string eventName)
            where THandler : IDynamicIntegrationEventHandler
        {
            DoAddSubscription(typeof(THandler), eventName, isDynamic: true);
        }

        public void AddSubscription<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            var eventName = GetEventKey<TEvent>();
            DoAddSubscription(typeof(THandler), eventName, isDynamic: false);
            eventTypes.Add(typeof(TEvent));
        }

        private void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                handlers.Add(eventName, new List<SubscriptionInfo>());
            }

            if (handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }

            var subsInfo = isDynamic ? SubscriptionInfo.Dynamic(handlerType) : SubscriptionInfo.Typed(handlerType);
            handlers[eventName].Add(subsInfo);
            RaiseOnAddSubscription(eventName);
        }

        private void RaiseOnAddSubscription(string eventName)
        {
            OnAddSubscription?.Invoke(this, eventName);
        }

        public void RemoveDynamicSubscription<THandler>(string eventName)
            where THandler : IDynamicIntegrationEventHandler
        {
            var handlerToRemove = FindDynamicSubscriptionToRemove<THandler>(eventName);
            DoRemoveHandler(eventName, handlerToRemove);
        }

        public void RemoveSubscription<T, THandler>()
            where THandler : IIntegrationEventHandler<T>
            where T : IntegrationEvent
        {
            var handlerToRemove = FindSubscriptionToRemove<T, THandler>();
            var eventName = GetEventKey<T>();
            DoRemoveHandler(eventName, handlerToRemove);
        }

        private void DoRemoveHandler(string eventName, SubscriptionInfo subsToRemove)
        {
            if (subsToRemove == null)
            {
                return;
            }

            handlers[eventName].Remove(subsToRemove);
            RaiseOnRemoveSubscription(eventName);

            if (!handlers[eventName].Any())
            {
                handlers.Remove(eventName);

                var eventType = eventTypes.SingleOrDefault(e => e.Name == eventName);

                if (eventType != null)
                {
                    eventTypes.Remove(eventType);
                }

                RaiseOnEventRemoved(eventName);
            }
        }

        private void RaiseOnRemoveSubscription(string eventName)
        {
            OnRemoveSubscription?.Invoke(this, eventName);
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return GetHandlersForEvent(key);
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => handlers[eventName];

        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;

            if (handler != null)
            {
                OnEventRemoved(this, eventName);
            }
        }

        private SubscriptionInfo FindDynamicSubscriptionToRemove<THandler>(string eventName)
            where THandler : IDynamicIntegrationEventHandler
        {
            return DoFindSubscriptionToRemove(eventName, typeof(THandler));
        }

        private SubscriptionInfo FindSubscriptionToRemove<T, THandler>()
             where T : IntegrationEvent
             where THandler : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();
            return DoFindSubscriptionToRemove(eventName, typeof(THandler));
        }

        private SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                return null;
            }

            return handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);

        }

        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return HasSubscriptionsForEvent(key);
        }

        public bool HasSubscriptionsForEvent(string eventName) => handlers.ContainsKey(eventName);

        public Type GetEventTypeByName(string eventName) => eventTypes.SingleOrDefault(t => t.Name == eventName);

        public string GetEventKey<T>()
        {
            return typeof(T).Name;
        }
    }
}
