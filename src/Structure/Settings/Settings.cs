using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Structure.Settings
{
    public abstract class Settings<TSetting>
        where TSetting : ISetting
    {
        protected readonly ISettingsManager<TSetting> settingsManager;

        public Settings(ISettingsManager<TSetting> settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        protected virtual T Get<T>(T defaultValue = default, [CallerMemberName]string name = null)
        {
            return settingsManager.Get<T>(name, defaultValue);
        }
        protected virtual void Set<T>(T value, [CallerMemberName]string name = null)
        {
            settingsManager.Set<T>(name, value);
        }

        public virtual async Task SaveChangesAsync()
        {
            await settingsManager.SaveChangesAsync();
        }
    }
}
