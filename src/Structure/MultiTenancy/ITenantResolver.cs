namespace Structure.MultiTenancy
{
    public interface ITenantResolver
    {
        TenantResolveResult ResolveTenantIdOrName();
    }
}
