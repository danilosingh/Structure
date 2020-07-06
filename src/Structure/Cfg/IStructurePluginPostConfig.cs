using System;

namespace Structure.Cfg
{
    public interface IStructurePluginPostConfig
    {
        void Post(IServiceProvider serviceProvider);
    }
}
