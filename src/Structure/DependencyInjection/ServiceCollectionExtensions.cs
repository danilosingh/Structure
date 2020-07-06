using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Structure.AspNetCore.Cfg;
using Structure.Cfg;
using Structure.Helpers;
using System;
using System.Linq;
using System.Reflection;

namespace Structure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddStructure(this IServiceCollection services, Action<IStructureAppBuilder> configureBuilder)
        {
            var configuration = GetStatupConfiguration();
            services.AddStructure(null, configureBuilder);
        }

        private static IConfiguration GetStatupConfiguration()
        {
            var configuration = Assembly.GetEntryAssembly().GetTypes()
                .Where(c => c.Name == "Startup" && c.IsPublic)
                .Select(c => TypeHelper.GetPropertyInfo(c, "Configuration")?.GetValue(null, null) as IConfiguration)
                .Where(c => c != null)
                .FirstOrDefault();

            if (configuration == null)
            {
                throw new NotSupportedException("could not find an instance of IConfiguration");
            }

            return configuration;
        }

        public static void AddStructure(this IServiceCollection services, IConfiguration configuration, Action<IStructureAppBuilder> configureBuilder)
        {
            var builder = new StructureAppBuilder(services, configuration);
            configureBuilder.Invoke(builder);
            builder.AddDefaultServices();
            builder.RegisterServices();
            services.AddSingleton(typeof(IStructureAppBuilder), builder);
        }

        public static IServiceCollection Configure<TOptions, TDependency>(this IServiceCollection serviceCollection, Action<TOptions, TDependency> configureOptions) where TOptions : class, new()
        {
            serviceCollection.AddSingleton<IConfigureOptions<TOptions>, ConfigureOptionsWithDependencyWrapper<TOptions, TDependency>>();
            serviceCollection.Configure<ConfigureOptionsWithDependency<TOptions, TDependency>>(options => { options.Action = configureOptions; });
            return serviceCollection;
        }

        public static T AddSingletonOption<T>(this IServiceCollection services, IConfiguration configuration, string section)
            where T : class, new()
        {
            var opts = new T();
            configuration.GetSection(section).Bind(opts);
            services.AddSingleton(Options.Create<T>(opts));
            return opts;
        }

        public static void AddScoped(this IServiceCollection services, Type[] serviceTypes, Type implementationType)
        {
            foreach (var serviceType in serviceTypes)
            {
                services.AddScoped(serviceType, implementationType);
            }
        }

        public static IServiceCollection AddOrReplace<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            var serviceType = typeof(TService);
            return services.AddOrReplace(serviceType, typeof(TImplementation), lifetime);
        }

        public static IServiceCollection AddOrReplace(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            var existingDescriptor = services.FirstOrDefault(d => d.ServiceType == serviceType);

            if (existingDescriptor != null)
            {
                services.Remove(existingDescriptor);
            }

            services.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));
            return services;
        }
    }
}
