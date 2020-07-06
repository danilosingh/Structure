using Structure.Cfg;

namespace Structure.Identity
{
    public static class StructurePluginCollectionExtensions
    {
        public static StructurePluginCollection AddIdentity(this StructurePluginCollection plugins)
        {
            plugins.Add(new IdentityStructurePlugin());
            return plugins;
        }
    }
}
