using Microsoft.AspNetCore.Http;
using Structure.MultiTenancy;
using System;
using System.Threading.Tasks;

namespace Structure.AspNetCore.MultiTenancy
{
    public class MultiTenancyMiddleware : IMiddleware
    {
        private readonly ITenantResolver tenantResolver;
        private readonly ITenantStore tenantStore;
        private readonly ICurrentTenant currentTenant;

        public MultiTenancyMiddleware(
            ITenantResolver tenantResolver, 
            ITenantStore tenantStore, 
            ICurrentTenant currentTenant)
        {
            this.tenantResolver = tenantResolver;
            this.tenantStore = tenantStore;
            this.currentTenant = currentTenant;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var resolveResult = tenantResolver.ResolveTenantIdOrName();

            TenantConfiguration tenant = null;

            if (resolveResult.TenantIdOrName != null)
            {
                tenant = await FindTenantAsync(resolveResult.TenantIdOrName);

                if (tenant == null)
                {
                    throw new StructureException("There is no tenant with given tenant id or name: " + resolveResult.TenantIdOrName);
                }
            }

            using (currentTenant.Change(tenant?.Id, tenant?.Name))
            {
                await next(context);
            }
        }

        private async Task<TenantConfiguration> FindTenantAsync(string tenantIdOrName)
        {
            if (Guid.TryParse(tenantIdOrName, out var parsedTenantId))
            {
                return await tenantStore.FindAsync(parsedTenantId);
            }
            else
            {
                return await tenantStore.FindAsync(tenantIdOrName);
            }
        }
    }
}
