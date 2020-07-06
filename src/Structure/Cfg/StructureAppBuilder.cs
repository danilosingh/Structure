using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Structure.Cfg
{
    public class StructureAppBuilder : IStructureAppBuilder
    {
        public StructureModuleTypeCollection Modules { get; }
        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }
        public StructureEntityTypes EntityTypes { get; }
        public StructurePluginCollection Plugins { get; }

        public StructureAppBuilder(IServiceCollection services, IConfiguration configuration)
        {
            Services = services;
            Configuration = configuration;
            Modules = new StructureModuleTypeCollection();
            EntityTypes = new StructureEntityTypes();
            Plugins = new StructurePluginCollection();
        }

        //TODO: remove IDependencyResolverFactory
        public virtual void Build(IServiceProvider serviceProvider)
        {
            EntityTypes.EnsureValidTypes();
            
            foreach (var item in Plugins.OfType<IStructurePluginPostConfig>())
            {
                item.Post(serviceProvider);
            }
        }

        internal void RegisterServices()
        {
            RegisterPluginsServices();
            RegisterModulesServices();
        }

        private void RegisterPluginsServices()
        {
            foreach (var plugin in Plugins)
            {
                plugin.Configure(this);
            }
        }

        private void RegisterModulesServices()
        {
            foreach (var moduleType in Modules)
            {
                var module = (IStructureModule)Activator.CreateInstance(moduleType);
                module.ConfigureServices(Services);
            }
        }
    }
}
