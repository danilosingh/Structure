using Structure.Nhibernate.Mapping;
using Structure.Tests.Shared.Domain.Entities;

namespace Structure.AspNetCoreDemo.Core.Mappings
{
    public class SettingMap : EntityClassMap<UserSetting>
    {
        protected override void PerformMapping()
        {
            Id(c => c.Name);
            Map(c => c.Value).CustomType<string>();
        }
    }
}
