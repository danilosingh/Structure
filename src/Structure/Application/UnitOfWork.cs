using Microsoft.Extensions.DependencyInjection;
using Structure.Data;
using Structure.Messaging.EventBus.Abstractions;
using System;
using System.Threading.Tasks;

namespace Structure.Application
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;
        private readonly IServiceProvider serviceProvider;
        private readonly IDataContext dataContext;
        private readonly ITransactionManager transactionManager;

        public UnitOfWorkStatus Status { get; private set; }
        public UnitOfWorkOptions Options { get; private set; }

        public event UnitOfWorkEventHandler Completed;
        public event UnitOfWorkEventHandler Failed;

        public UnitOfWork(IServiceProvider serviceProvider,
            IDataContext dataContext,
            ITransactionManager transactionManager)
        {
            Status = UnitOfWorkStatus.Created;
            this.serviceProvider = serviceProvider;
            this.dataContext = dataContext;
            this.transactionManager = transactionManager;
        }

        public async Task CompleteAsync()
        {
            await SaveChangesAsync();

            if (transactionManager.TransactionActive)
            {
                await transactionManager.CommitAsync();
            }

            if (Options.PublishIntegrationEventsOnComplete)
            {
                serviceProvider.GetService<IDistributedEventStore>().PublishEvents();
            }

            Status = UnitOfWorkStatus.Completed;
            OnCompleted();
        }

        protected virtual void OnCompleted()
        {
            Completed?.Invoke(this);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed || Status == UnitOfWorkStatus.Completed)
            {
                return;
            }

            disposed = true;
            OnDisposing();
        }

        protected virtual void OnDisposing()
        {
            Status = UnitOfWorkStatus.Failed;
            Failed?.Invoke(this);
        }

        public void Begin(UnitOfWorkOptions options)
        {
            Options = options;
            transactionManager.BeginTransaction();
            Status = UnitOfWorkStatus.InProgress;
        }

        public async Task SaveChangesAsync()
        {
            await dataContext.SaveChangesAsync();
        }
    }
}
