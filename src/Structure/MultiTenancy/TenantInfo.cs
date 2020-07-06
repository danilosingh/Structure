using System;

namespace Structure.MultiTenancy
{
    public class TenantInfo
    {
        public Guid? TenantId { get; }
        public string Name { get; }

        public TenantInfo(Guid? tenantId, string name = null)
        {
            TenantId = tenantId;
            Name = name;
        }
    }
}