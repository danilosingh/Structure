namespace Structure.MultiTenancy
{
    public class DefaultTenantStoreOptions
    {
        public TenantConfiguration[] Tenants { get; set; }

        public DefaultTenantStoreOptions()
        {
            Tenants = new TenantConfiguration[0];
        }
    }
}
