using System;
using System.Threading.Tasks;

namespace Structure.MultiTenancy
{
    public interface ITenantStore
    {
        Task<TenantConfiguration> FindAsync(string name);

        Task<TenantConfiguration> FindAsync(Guid id);

        TenantConfiguration Find(string name);

        TenantConfiguration Find(Guid id);
    }
}
