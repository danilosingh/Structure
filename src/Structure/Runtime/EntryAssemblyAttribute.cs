using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Structure.Runtime
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class EntryAssemblyAttribute : Attribute
    {
        private static readonly Lazy<Assembly> EntryAssemblyLazy = new Lazy<Assembly>(GetEntryAssemblyLazily);

        public static Assembly GetEntryAssembly()
        {
            return EntryAssemblyLazy.Value;
        }

        private static Assembly GetEntryAssemblyLazily()
        {
            return Assembly.GetEntryAssembly() ?? FindEntryAssemblyInCurrentAppDomain();
        }

        private static Assembly FindEntryAssemblyInCurrentAppDomain()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var entryAssemblies = new List<Assembly>();

            foreach (var assembly in assemblies)
            {
                var attribute = assembly.GetCustomAttributes().OfType<EntryAssemblyAttribute>().SingleOrDefault();

                if (attribute != null)
                {
                    entryAssemblies.Add(assembly);
                }
            }

            return entryAssemblies.SingleOrDefault();
        }
    }
}
