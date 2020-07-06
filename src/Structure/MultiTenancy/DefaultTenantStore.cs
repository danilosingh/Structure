using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Structure.MultiTenancy
{
    public class DefaultTenantStore : ITenantStore
    {
        private readonly DefaultTenantStoreOptions options;

        public DefaultTenantStore(IOptionsSnapshot<DefaultTenantStoreOptions> options)
        {
            this.options = options.Value;
        }

        public Task<TenantConfiguration> FindAsync(string name)
        {
            return Task.FromResult(Find(name));
        }

        public Task<TenantConfiguration> FindAsync(Guid id)
        {
            return Task.FromResult(Find(id));
        }

        public TenantConfiguration Find(string name)
        {
            return options.Tenants?.FirstOrDefault(t => t.Name == name);
        }

        public TenantConfiguration Find(Guid id)
        {
            return options.Tenants?.FirstOrDefault(t => t.Id == id);
        }
    }
}