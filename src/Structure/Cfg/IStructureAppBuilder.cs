using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structure.Cfg
{
    public interface IStructureAppBuilder
    {
        IConfiguration Configuration { get; }
        IServiceCollection Services { get; }
        StructureModuleTypeCollection Modules { get; }
        StructureEntityTypes EntityTypes { get; }
        StructurePluginCollection Plugins { get; }
        void Build(IServiceProvider serviceProvider);        
    }
}
