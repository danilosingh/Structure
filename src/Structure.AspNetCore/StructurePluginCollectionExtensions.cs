using Structure.Cfg;
using System;

namespace Structure.AspNetCore.Extensions
{
    public static class StructurePluginCollectionExtensions
    {
        public static StructurePluginCollection AddAspNetCore(this StructurePluginCollection plugins, 
            Action<AspNetCoreOptions> setupAction = null)
        {
            plugins.Add(new AspNetStructurePlugin(setupAction));
            return plugins;
        }
    }
}
