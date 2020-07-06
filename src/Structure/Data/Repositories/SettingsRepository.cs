using Structure.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Structure.Data.Repositories
{
    public class SettingsRepository<TSetting> : RepositoryBase, ISettingsRepository<TSetting>
        where TSetting : class, ISetting
    {
        public SettingsRepository(IDataContext context) : base(context)
        {
        }

        public IList<TSetting> GetAllSettings()
        {
            return context.Query<TSetting>().ToList();
        }

        public TSetting GetSetting(string name)
        {
            return context.Query<TSetting>()
                .Where(c => c.Name == name)
                .FirstOrDefault();
        }

        public async Task SaveAsync(TSetting setting)
        {
            if (!context.Query<TSetting>().Any(c => c.Name == setting.Name))
            {
                await context.CreateAsync(setting);
            }
            else
            {
                await context.UpdateAsync(setting);
            }
        }
    }
}
