using Structure.Cfg;

namespace Structure.Nhibernate
{
    public static class StructurePluginCollectionExtensions
    {
        public static StructurePluginCollection AddNhibernate(this StructurePluginCollection plugins)
        {
            plugins.Add(new NhibernateStructurePlugin());
            return plugins;
        }
    }
}
