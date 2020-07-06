using System.Collections.Generic;

namespace Structure.Settings
{
    public interface ISettingsCache<TSetting>
    {
        TSetting Get(string name);
        void Set(TSetting setting);
        bool HasSetting(string name);
        IReadOnlyCollection<TSetting> GetAll();
    }
}
