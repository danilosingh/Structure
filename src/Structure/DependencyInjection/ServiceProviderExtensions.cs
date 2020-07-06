using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Structure.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static TOptions GetOptions<TOptions>(this IServiceProvider serviceProvider)
            where TOptions : class, new()
        {
            return serviceProvider.GetService<IOptions<TOptions>>().Value;
        }
    }
}
