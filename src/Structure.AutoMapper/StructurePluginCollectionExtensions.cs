using Structure.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Structure.AutoMapper
{
    public static class StructurePluginCollectionExtensions 
    {
        public static StructurePluginCollection AddAutoMapper(this StructurePluginCollection plugins, Action<List<Assembly>> configureAssemblies)
        {
            var assemblies = new List<Assembly>();
            configureAssemblies(assemblies);
            plugins.Add(new AutoMapperStructurePlugin(assemblies.ToArray()));
            return plugins;
        }

        public static StructurePluginCollection AddAutoMapper(this StructurePluginCollection plugins, Action<List<string>> configureAssemblies)
        {
            var assemblyStrings = new List<string>();
            configureAssemblies(assemblyStrings);
            var assemblies = assemblyStrings.Select(assemblyString => Assembly.Load(assemblyString)).ToList();
            plugins.Add(new AutoMapperStructurePlugin(assemblies.ToArray()));
            return plugins;
        }
    }
}
