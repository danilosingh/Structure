using Microsoft.Extensions.DependencyInjection;
using Structure.Application;
using Structure.Messaging.EventBus.Abstractions;
using System;
using System.Threading.Tasks;

namespace Structure.Messaging.EventBus
{
    public class UnitOfWorkEventBusProcessingContextProvider : IEventBusProcessingContextProvider
    {
        private readonly IServiceProvider serviceProvider;

        public UnitOfWorkEventBusProcessingContextProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IEventBusProcessingContext CreateContext()
        {
            return new UnitOfWorkEventBusProcessingContext(serviceProvider.CreateScope());
        }
    }

    public class UnitOfWorkEventBusProcessingContext : IEventBusProcessingContext
    {
        private readonly IServiceScope scope;

        public IUnitOfWorkManager UnitOfWorkManager { get; }
        public IUnitOfWork UnitOfWork { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }
        public UnitOfWorkEventBusProcessingContext(IServiceScope scope)
        {
            this.scope = scope;
            ServiceProvider = scope.ServiceProvider;
            UnitOfWorkManager = scope.ServiceProvider.GetService<IUnitOfWorkManager>();
            UnitOfWork = UnitOfWorkManager.Begin(new UnitOfWorkOptions());
            this.scope = scope;
        }

        public Task CompleteAsync()
        {
            return UnitOfWork.CompleteAsync();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            UnitOfWorkManager.Dispose();
            scope.Dispose();
        }
    }
}
