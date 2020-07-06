using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structure.Autofac
{
    public class AutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        private readonly Action<ContainerBuilder> _configurationAction;

        
        public AutofacServiceProviderFactory(Action<ContainerBuilder> configurationAction = null)
        {
            _configurationAction = configurationAction ?? (builder => { });
        }
        
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AutofacServiceProvider>().As<IServiceProvider>();
            builder.RegisterType<AutofacServiceScopeFactory>().As<IServiceScopeFactory>();

            AutofacRegistration.Register(builder, services);

            _configurationAction(builder);

            return builder;
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            if (containerBuilder == null) throw new ArgumentNullException(nameof(containerBuilder));

            var container = containerBuilder.Build();

            return new AutofacServiceProvider(container);
        }
    }
}
