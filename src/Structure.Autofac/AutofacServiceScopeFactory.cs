using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structure.Autofac
{
    internal class AutofacServiceScopeFactory : IServiceScopeFactory
    {
        private readonly ILifetimeScope lifetimeScope;

        public AutofacServiceScopeFactory(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        public IServiceScope CreateScope()
        {
            return new AutofacServiceScope(this.lifetimeScope.BeginLifetimeScope());
        }
    }
}
