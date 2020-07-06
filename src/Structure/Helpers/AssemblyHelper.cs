using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Structure.Helpers
{
    public static class AssemblyHelper
    {
        public static Guid GetGuid(Assembly assembly)
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(GuidAttribute), true);
            return new Guid(((GuidAttribute)attributes[0]).Value);
        }

        public static Guid GetEntryAssemblyGuid()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            object[] attributes = assembly.GetCustomAttributes(typeof(GuidAttribute), true);
            return new Guid(((GuidAttribute)attributes[0]).Value);
        }

        public static string GetTitle(Assembly assembly)
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), true);
            return ((AssemblyTitleAttribute)attributes[0]).Title;
        }

        public static Version GetFileVersion(Assembly assembly)
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true);
            return new Version(((AssemblyFileVersionAttribute)attributes[0]).Version);
        }

        public static string GetCompany(Assembly assembly)
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true);
            return ((AssemblyCompanyAttribute)attributes[0]).Company;            
        }

        public static Version GetAssemblyEntryVersion()
        {
            return Assembly.GetEntryAssembly().GetName().Version;
        }

        public static string GetNamespaceEntryAssembly(string concateStr = null)
        {
            return Assembly.GetEntryAssembly().GetName().Name + concateStr;
        }
    }
}
