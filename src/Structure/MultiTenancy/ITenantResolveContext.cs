using Structure.DependencyInjection;

namespace Structure.MultiTenancy
{
    public interface ITenantResolveContext : IServiceProviderAccessor
    {
        string TenantIdOrName { get; set; }

        bool Handled { get; set; }
    }
}