using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structure.Autofac
{
    internal class AutofacServiceScope : IServiceScope
    {
        private readonly ILifetimeScope lifetimeScope;

        public AutofacServiceScope(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
            ServiceProvider = this.lifetimeScope.Resolve<IServiceProvider>();
        }

        public IServiceProvider ServiceProvider { get; }

        public void Dispose()
        {
            lifetimeScope.Dispose();
        }
    }
}
