using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Structure.DependencyInjection;
using Structure.Extensions;
using Structure.MultiTenancy;
using System;

namespace Structure.AspNetCore.MultiTenancy
{
    public class RouteTenantResolveContributor : HttpTenantResolveContributorBase
    {
        public override string Name
        {
            get { return "Route"; }
        }

        protected override string GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext)
        {
            var options = context.ServiceProvider.GetOptions<RouteTenantResolverOptions>();

            var tenantId = Convert.ToString(httpContext.GetRouteValue(options.ParameterName));

            if (tenantId.IsNullOrEmpty() ||
               (!options.HostValue.IsNullOrEmpty() &&
                tenantId.ToLower() == options.HostValue?.ToLower()))
            {
                context.Handled = true;
                return null;
            }

            return tenantId;
        }
    }
}
