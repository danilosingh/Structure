using Structure.Helpers;
using System;
using System.Threading.Tasks;

namespace Structure.Settings
{
    public class SettingsManager<TSetting> : ISettingsManager<TSetting>
        where TSetting : class, ISetting
    {        
        private readonly ISettingsCache<TSetting> settingsCache;
        private readonly ISettingsRepository<TSetting> settingsRepository;

        public SettingsManager(ISettingsCache<TSetting> settingsCache, ISettingsRepository<TSetting> settingsRepository)
        {
            this.settingsCache = settingsCache;
            this.settingsRepository = settingsRepository;
        }


        public T Get<T>(string name = null, T defaultValue = default)
        {
            TSetting setting = GetSetting(name) ?? CreateSetting<T>(name, defaultValue);

            return TypeHelper.Convert<T>(setting.Value);
        }

        public void Set<T>(string name, T value)
        {
            var setting = GetSetting(name) ?? CreateSetting<T>(name, value);
            setting.Value = value;
        }

        protected TSetting CreateSetting<T>(string name, T value)
        {
            var setting = Activator.CreateInstance<TSetting>();
            setting.Name = name;
            setting.Value = value;

            settingsCache.Set(setting);

            return setting;
        }

        protected TSetting GetSetting(string name)
        {
            if (AvaliableInCache(name))
            {
                return GetSettingFromCache(name);
            }

            return GetSettingFromRepository(name);
        }

        protected TSetting GetSettingFromRepository(string name)
        {
            return settingsRepository.GetSetting(name);
        }

        protected TSetting GetSettingFromCache(string name)
        {
            return settingsCache.Get(name);
        }

        protected virtual void OnCreateSetting(TSetting setting)
        { }

        protected bool AvaliableInCache(string key)
        {
            return settingsCache.HasSetting(key);
        }

        public async Task SaveChangesAsync()
        {
            foreach (var setting in settingsCache.GetAll())
            {
                await settingsRepository.SaveAsync(setting);
            }
        }
    }
}
