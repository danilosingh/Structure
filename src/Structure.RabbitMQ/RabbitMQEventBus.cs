using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Structure.Infrastructure.Messaging.EventBus.Events;
using Structure.Messaging.EventBus.Abstractions;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Structure.RabbitMQ
{
    public class RabbitMQEventBus : AbstractEventBus, IDisposable
    {
        private readonly IRabbitMQPersistentConnection persistentConnection;
        private readonly RabbitMQEventBusOptions options;
        private readonly object threadLock = new object();
        private IModel consumerChannel;

        public RabbitMQEventBus(IServiceProvider serviceProvider, IOptions<RabbitMQEventBusOptions> options) : base(serviceProvider)
        {
            this.options = options.Value;
            this.persistentConnection = serviceProvider.GetService<IRabbitMQPersistentConnection>();
            this.subscriptionsManager.OnEventRemoved += SubsManagerOnEventRemoved;
            this.subscriptionsManager.OnAddSubscription += SubscriptionManagerOnAddSubscription;
            consumerChannel = CreateConsumerChannel();
        }

        public override void Publish(IntegrationEvent @event)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            var policy = Policy
                .Handle<BrokerUnreachableException>().Or<SocketException>()
                .WaitAndRetry(options.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    logger.LogError(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                });

            var eventName = @event.GetType().Name;

            logger.LogDebug("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.Id, eventName);

            using (var channel = persistentConnection.CreateModel())
            {

                logger.LogDebug("Declaring RabbitMQ exchange to publish event: {EventId}", @event.Id);

                channel.ExchangeDeclare(exchange: options.BrokerName, type: "direct");

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent

                    logger.LogDebug("Publishing event to RabbitMQ: {EventId}", @event.Id);

                    channel.BasicPublish(
                        exchange: options.BrokerName,
                        routingKey: eventName,
                        mandatory: true,
                        basicProperties: properties,
                        body: body);
                });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (consumerChannel != null)
            {
                consumerChannel.Dispose();
            }
        }

        private void StartBasicConsume()
        {
            logger.LogDebug("Starting RabbitMQ basic consume");

            if (consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(consumerChannel);

                consumer.Received += ConsumerReceived;

                consumerChannel.BasicConsume(
                    queue: options.QueueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                logger.LogDebug("StartBasicConsume can't call on consumerChannel == null");
            }
        }

        private IModel CreateConsumerChannel()
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            logger.LogDebug("Creating RabbitMQ consumer channel");

            var channel = persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: options.BrokerName,
                                    type: "direct");

            channel.QueueDeclare(queue: options.QueueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                lock (threadLock)
                {
                    logger.LogError(ea.Exception, "Recreating RabbitMQ consumer channel");

                    consumerChannel.Dispose();
                    consumerChannel = CreateConsumerChannel();
                    StartBasicConsume();
                }
            };

            return channel;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (!subscriptionsManager.HasSubscriptionsForEvent(eventName))
            {
                logger.LogDebug("No subscription for RabbitMQ event: {0}", eventName);
                return;
            }

            logger.LogDebug("Processing RabbitMQ event: {0}", eventName);

            var contextProvider = serviceProvider.GetService<IEventBusProcessingContextProvider>();

            using (var context = contextProvider.CreateContext())
            {
                var subscriptions = subscriptionsManager.GetHandlersForEvent(eventName);

                foreach (var subscription in subscriptions)
                {
                    if (subscription.IsDynamic)
                    {
                        if (!(context.ServiceProvider.GetService(subscription.HandlerType) is IDynamicIntegrationEventHandler handler))
                        {
                            continue;
                        }

                        dynamic eventData = JObject.Parse(message);

                        await Task.Yield();
                        await handler.Handle(eventData);
                    }
                    else
                    {
                        var handler = context.ServiceProvider.GetService(subscription.HandlerType);

                        if (handler == null)
                        {
                            continue;
                        }

                        var eventType = subscriptionsManager.GetEventTypeByName(eventName);
                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        await Task.Yield();
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                    }
                }

                await context.CompleteAsync();
            }
        }

        private void SubsManagerOnEventRemoved(object sender, string eventName)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            using (var channel = persistentConnection.CreateModel())
            {
                channel.QueueUnbind(queue: options.QueueName,
                    exchange: options.BrokerName,
                    routingKey: eventName);

                if (subscriptionsManager.IsEmpty)
                {
                    consumerChannel.Close();
                }
            }
        }

        private void SubscriptionManagerOnAddSubscription(object sender, string eventName)
        {
            if (subscriptionsManager.GetHandlersForEvent(eventName).Count() == 1)
            {
                if (!persistentConnection.IsConnected)
                {
                    persistentConnection.TryConnect();
                }

                using (var channel = persistentConnection.CreateModel())
                {
                    channel.QueueBind(queue: options.QueueName,
                                      exchange: options.BrokerName,
                                      routingKey: eventName);
                }
            }

            StartBasicConsume();
        }

        private async Task ConsumerReceived(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                {
                    throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                }

                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ERROR Processing message \"{0}\"", message);
            }

            consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }
    }
}
