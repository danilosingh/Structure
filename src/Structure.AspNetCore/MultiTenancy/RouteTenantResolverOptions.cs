namespace Structure.AspNetCore.MultiTenancy
{
    public class RouteTenantResolverOptions
    {
        public string ParameterName { get; set; } = "tenantId";
        public string HostValue { get; set; } = "host";
    }
}
