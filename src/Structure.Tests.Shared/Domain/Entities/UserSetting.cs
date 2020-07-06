using Structure.Settings;

namespace Structure.Tests.Shared.Domain.Entities
{
    public class UserSetting : ISetting
    {
        public virtual string Name { get; set; }
        public virtual object Value { get; set; }
    }
}
