using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Structure.MultiTenancy
{
    public class TenantResolver : ITenantResolver
    {
        private readonly IServiceProvider serviceProvider;
        private readonly TenantResolveOptions options;

        public TenantResolver(IOptions<TenantResolveOptions> options, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.options = options.Value;
        }

        public TenantResolveResult ResolveTenantIdOrName()
        {
            var result = new TenantResolveResult();

            using (var serviceScope = serviceProvider.CreateScope())
            {
                var context = new TenantResolveContext(serviceScope.ServiceProvider);

                foreach (var tenantResolver in options.TenantResolvers)
                {
                    tenantResolver.Resolve(context);

                    result.AppliedResolvers.Add(tenantResolver.Name);

                    if (context.HasResolvedTenantOrHost())
                    {
                        result.TenantIdOrName = context.TenantIdOrName;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
