using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Structure.DependencyInjection;

namespace Structure.AspNetCore.Extensions
{
    public static class ServiceProviderAccessorExtensions
    {
        public static HttpContext GetHttpContext(this IServiceProviderAccessor serviceProviderAccessor)
        {
            return serviceProviderAccessor.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        }
    }
}
