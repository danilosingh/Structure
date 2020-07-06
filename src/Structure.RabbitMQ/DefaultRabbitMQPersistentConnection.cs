using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;

namespace Structure.RabbitMQ
{
    public class DefaultRabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        private readonly RabbitMQEventBusOptions options;
        private readonly IConnectionFactory connectionFactory;
        private readonly ILogger<IRabbitMQPersistentConnection> logger;
        private readonly object sync_root = new object();
        private readonly int retryCount;
        private IConnection connection;
        private bool disposed;

        public bool IsConnected
        {
            get
            {
                return connection != null && connection.IsOpen && !disposed;
            }
        }

        public DefaultRabbitMQPersistentConnection(IConnectionFactory connectionFactory, 
            IOptions<RabbitMQEventBusOptions> options, 
            ILogger<IRabbitMQPersistentConnection> logger)
        {
            this.options = options.Value;
            this.connectionFactory = connectionFactory;
            this.logger = logger;
        }

        public virtual IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return connection.CreateModel();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            try
            {
                connection.Dispose();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error on dispose RabbitMQ Connection");
            }
        }

        public virtual bool TryConnect()
        {
            logger.LogDebug("RabbitMQ Client is trying to connect");

            lock (sync_root)
            {
                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        logger.LogError(ex, "RabbitMQ Client could not connect after {0}s ({1})", $"{time.TotalSeconds:n1}", ex.Message);
                    }
                );

                policy.Execute(() =>
                {
                    connection = connectionFactory
                          .CreateConnection();
                });

                if (IsConnected)
                {
                    connection.ConnectionShutdown += OnConnectionShutdown;
                    connection.CallbackException += OnCallbackException;
                    connection.ConnectionBlocked += OnConnectionBlocked;

                    return true;
                }
                else
                {
                    logger.LogError("FATAL ERROR: RabbitMQ connections could not be created and opened");
                    return false;
                }
            }
        }

        protected virtual void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (disposed)
            {
                return;
            }

            logger.LogDebug("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }

        protected virtual void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (disposed)
            {
                return;
            }

            logger.LogDebug("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        protected virtual void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (disposed)
            {
                return;
            }

            logger.LogDebug("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }
    }
}
