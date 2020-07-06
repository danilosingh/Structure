using System.Threading.Tasks;

namespace Structure.Settings
{
    public interface ISettingsManager<TSetting>
        where TSetting : ISetting
    {
        T Get<T>(string name = null, T defaultValue = default);
        void Set<T>(string name, T value);
        Task SaveChangesAsync();
    }
}
