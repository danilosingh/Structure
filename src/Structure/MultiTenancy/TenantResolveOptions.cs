using System.Collections.Generic;

namespace Structure.MultiTenancy
{
    public class TenantResolveOptions
    {
        public List<ITenantResolveContributor> TenantResolvers { get; }

        public TenantResolveOptions()
        {
            TenantResolvers = new List<ITenantResolveContributor>
            {
                new CurrentUserTenantResolveContributor()
            };
        }
    }
}