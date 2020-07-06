namespace Structure.RabbitMQ
{
    public class RabbitMQEventBusOptions
    {
        public string BrokerName { get; set; }
        public string QueueName { get; set; }
        public int RetryCount { get; set; } = 5;
        public bool DispatchConsumersAsync { get; set; } = true;
        public string HostName { get; set; } = "localhost";
    }
}
