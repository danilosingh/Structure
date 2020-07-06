using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Structure.AspNetCore.MultiTenancy;
using Structure.Cfg;
using Structure.DependencyInjection;
using Structure.MultiTenancy;

namespace Structure.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseStructure(this IApplicationBuilder builder)
        {
            var structureBuilder = builder.ApplicationServices.GetService<IStructureAppBuilder>();
            structureBuilder.Build(builder.ApplicationServices);
            AddMultiTenancyMiddleware(builder);
        }

        private static void AddMultiTenancyMiddleware(IApplicationBuilder builder)
        {
            var multiTenantOptions = builder.ApplicationServices.GetOptions<MultiTenancyOptions>();

            if (multiTenantOptions.IsEnabled)
            {
                builder.UseMiddleware<MultiTenancyMiddleware>();
            }
        }
    }
}


