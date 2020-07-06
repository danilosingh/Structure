using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structure.Application
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        protected readonly IServiceProvider serviceProvider;
        protected IUnitOfWork activeUnitOfWork;
        private bool isDisposed;

        public bool HasActiveUnitOfWork()
        {
            return activeUnitOfWork != null;
        }

        public UnitOfWorkManager(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IUnitOfWork Begin()
        {
            return Begin(new UnitOfWorkOptions());
        }

        public IUnitOfWork Begin(UnitOfWorkOptions options)
        {
            return CreateUnitOfWork(options);
        }

        protected virtual IUnitOfWork CreateUnitOfWork(UnitOfWorkOptions options)
        {
            if (HasActiveUnitOfWork())
            {
                return new InnerUnitOfWork();
            }

            activeUnitOfWork = serviceProvider.GetService<IUnitOfWork>();
            activeUnitOfWork.Begin(options);
            return activeUnitOfWork;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;
            activeUnitOfWork?.Dispose();
            activeUnitOfWork = null;
        }
    }
}
