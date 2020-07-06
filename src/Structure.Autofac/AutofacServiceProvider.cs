using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structure.Autofac
{
    public class AutofacServiceProvider : IServiceProvider, ISupportRequiredService, IDisposable
    {
        private bool disposed = false;
        public ILifetimeScope LifetimeScope { get; }

        public AutofacServiceProvider(ILifetimeScope lifetimeScope)
        {
            LifetimeScope = lifetimeScope;
        }

        public object GetRequiredService(Type serviceType)
        {
            return LifetimeScope.Resolve(serviceType);
        }

        public object GetService(Type serviceType)
        {
            return LifetimeScope.ResolveOptional(serviceType);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                if (disposing)
                {
                    LifetimeScope.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
