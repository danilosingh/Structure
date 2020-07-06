using Structure.Settings;
using Structure.Tests.Shared.Domain.Entities;

namespace Structure.AspNetCoreDemo.Core
{
    public class UserSettings : Settings<UserSetting>
    {
        public UserSettings(ISettingsManager<UserSetting> settingsManager) : base(settingsManager)
        {
        }

        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }
    }
}
