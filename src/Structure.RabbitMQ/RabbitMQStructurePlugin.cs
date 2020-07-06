using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Structure.Cfg;
using Structure.DependencyInjection;
using Structure.Messaging.EventBus.Abstractions;
using System;

namespace Structure.RabbitMQ
{
    public class RabbitMQStructurePlugin : IStructurePlugin
    {
        private readonly Action<RabbitMQEventBusOptions> configureOptions;

        public RabbitMQStructurePlugin(Action<RabbitMQEventBusOptions> configureOptions)
        {
            this.configureOptions = configureOptions;
        }

        public void Configure(IStructureAppBuilder builder)
        {
            if (configureOptions != null)
            {
                builder.Services.Configure(configureOptions);
            }

            builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();
            builder.Services.AddSingleton<IRabbitMQPersistentConnection, DefaultRabbitMQPersistentConnection>();

            builder.Services.AddSingleton<IConnectionFactory>((c) =>
            {
                var options = c.GetOptions<RabbitMQEventBusOptions>();
                return new ConnectionFactory() { HostName = options.HostName, DispatchConsumersAsync = options.DispatchConsumersAsync };
            });
        }
    }
}
