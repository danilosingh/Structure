using Structure.Cfg;
using System;

namespace Structure.RabbitMQ
{
    public static class StructurePluginCollectionExtensions
    {
        public static StructurePluginCollection AddRabbitMQ(this StructurePluginCollection plugins, Action<RabbitMQEventBusOptions> configureOptions)
        {
            plugins.Add(new RabbitMQStructurePlugin(configureOptions));
            return plugins;
        }
    }
}
