using System.Collections.Generic;

namespace Structure.MultiTenancy
{
    public class TenantResolveResult
    {
        public string TenantIdOrName { get; set; }

        public List<string> AppliedResolvers { get; }

        public TenantResolveResult()
        {
            AppliedResolvers = new List<string>();
        }
    }
}
