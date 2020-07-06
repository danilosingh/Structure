using Structure.Collections.Extensions;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Structure.Settings
{
    public class SettingsCache<TSetting> : ISettingsCache<TSetting>
        where TSetting : ISetting
    {
        private readonly ConcurrentDictionary<string, TSetting> cache;

        public SettingsCache()
        {
            cache = new ConcurrentDictionary<string, TSetting>();
        }

        public TSetting Get(string name)
        {
            cache.TryGetValue(name, out TSetting setting);
            return setting;
        }

        public IReadOnlyCollection<TSetting> GetAll()
        {
            return cache.Values.AsReadOnly();
        }

        public bool HasSetting(string name)
        {
            return cache.ContainsKey(name);
        }

        public void Set(TSetting setting)
        {
            cache.AddOrUpdate(setting.Name, setting, (name, oldValue) => setting);
        }
    }
}
