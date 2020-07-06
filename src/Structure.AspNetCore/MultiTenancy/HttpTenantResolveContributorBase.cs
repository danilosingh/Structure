using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Structure.AspNetCore.Extensions;
using Structure.Extensions;
using Structure.MultiTenancy;
using System;

namespace Structure.AspNetCore.MultiTenancy
{
    public abstract class HttpTenantResolveContributorBase : ITenantResolveContributor
    {
        public abstract string Name { get; }

        public virtual void Resolve(ITenantResolveContext context)
        {
            var httpContext = context.GetHttpContext();

            if (httpContext == null)
            {
                return;
            }

            try
            {
                ResolveFromHttpContext(context, httpContext);
            }
            catch (Exception e)
            {
                context.ServiceProvider
                    .GetRequiredService<ILogger<HttpTenantResolveContributorBase>>()
                    .LogWarning(e.ToString());
            }
        }

        protected virtual void ResolveFromHttpContext(ITenantResolveContext context, HttpContext httpContext)
        {
            var tenantIdOrName = GetTenantIdOrNameFromHttpContextOrNull(context, httpContext);
            if (!tenantIdOrName.IsNullOrEmpty())
            {
                context.TenantIdOrName = tenantIdOrName;
            }
        }

        protected abstract string GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext);
    }
}
