using System.Collections.Generic;
using System.Threading.Tasks;

namespace Structure.Settings
{
    public interface ISettingsRepository<TSetting>
        where TSetting : ISetting
    {
        TSetting GetSetting(string name);
        IList<TSetting> GetAllSettings();
        Task SaveAsync(TSetting setting);
    }
}
