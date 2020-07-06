using Microsoft.Extensions.DependencyInjection;
using Structure.Application;
using Structure.Auditing;
using Structure.Cfg;
using Structure.Data.Filtering;
using Structure.Data.Repositories;
using Structure.Domain.Notifications;
using Structure.ExceptionHandling;
using Structure.Infrastructure.Messaging.EventBus;
using Structure.Localization;
using Structure.Messaging.EventBus;
using Structure.Messaging.EventBus.Abstractions;
using Structure.MultiTenancy;
using Structure.Runtime.Caching;
using Structure.Runtime.Caching.Memory;
using Structure.Security.Authorization;
using Structure.Session;
using Structure.Settings;
using Structure.Utils;

namespace Structure
{
    public static class StructureBuilderExtensions
    {
        internal static IStructureAppBuilder AddDefaultServices(this IStructureAppBuilder builder)
        {
            return builder.AddCache<InMemoryCacheManager>()
                .AddDefaultSession()
                .AddDefaultUnitOfWork()
                .AddDefaultAuthorizationService()
                .AddDefaultLocalizationServices()
                .AddDefaultStores()
                .AddAuditServices()
                .AddSettingsServices()
                .AddMultiTenancyServices()
                .AddMessaging()
                .AddOtherServices();
        }

        internal static IStructureAppBuilder AddDefaultLocalizationServices(this IStructureAppBuilder builder)
        {
            builder.Services.AddScoped<ILocalizer, DefaultLocalizer>();
            builder.Services.AddScoped<ILocalizableContext, LocalizableContext>();
            return builder;
        }

        internal static IStructureAppBuilder AddAuditServices(this IStructureAppBuilder builder)
        {
            builder.Services.AddScoped<IEntityPropertySetter, EntityPropertySetter>();
            return builder;
        }

        internal static IStructureAppBuilder AddSettingsServices(this IStructureAppBuilder builder)
        {
            builder.Services.AddScoped(typeof(ISettingsRepository<>), typeof(SettingsRepository<>));
            builder.Services.AddScoped(typeof(ISettingsManager<>), typeof(SettingsManager<>));
            builder.Services.AddScoped(typeof(ISettingsCache<>), typeof(SettingsCache<>));
            return builder;
        }

        internal static IStructureAppBuilder AddOtherServices(this IStructureAppBuilder builder)
        {
            builder.Services.AddScoped<INotificationCollection, NotificationCollection>();
            builder.Services.AddSingleton<IPluralizationService, PluralizationService>();
            builder.Services.AddScoped<IErrorInfoBuilder, ErrorInfoBuilder>();            
            builder.Services.AddSingleton(typeof(IDataFilterHandler), typeof(DataFilterHandler));
            builder.Services.AddTransient<SoftDeleteFilter>();
            builder.Services.Configure<DataFilterOptions>(c => c.Filters.Add(typeof(SoftDeleteFilter), new DataFilterState(true)));
            return builder;
        }

        internal static IStructureAppBuilder AddDefaultUnitOfWork(this IStructureAppBuilder builder)
        {
            builder.Services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            return builder;
        }

        internal static IStructureAppBuilder AddDefaultStores(this IStructureAppBuilder builder)
        {
            builder.Services.AddSingleton<IPermissionStore, PermissionStore>();
            return builder;
        }

        internal static IStructureAppBuilder AddDefaultSession(this IStructureAppBuilder builder)
        {
            var claimAppSessionType = typeof(ClaimsAppSession);
            builder.Services.AddScoped(claimAppSessionType);
            builder.Services.AddScoped(typeof(IAppSession), provider => provider.GetService(claimAppSessionType));
            builder.Services.AddScoped<ICurrentUser, CurrentUser>();
            return builder;
        }

        internal static IStructureAppBuilder AddMultiTenancyServices(this IStructureAppBuilder builder)
        {
            builder.Services.AddScoped<ICurrentTenant, CurrentTenant>();
            builder.Services.AddScoped<ITenantStore, DefaultTenantStore>();
            builder.Services.AddScoped<ITenantResolver, TenantResolver>();
            builder.Services.AddSingleton<ICurrentTenantAccessor, AsyncLocalCurrentTenantAccessor>();
            builder.Services.AddTransient<MultiTenantFilter>();
            builder.Services.Configure<DataFilterOptions>(c => c.Filters.Add(typeof(MultiTenantFilter), new DataFilterState(true)));
            return builder;
        }

        internal static IStructureAppBuilder AddDefaultAuthorizationService(this IStructureAppBuilder builder)
        {
            var type = typeof(AuthorizationService);
            builder.Services.AddScoped(type);
            builder.Services.AddScoped(typeof(IAuthorizationService), provider => provider.GetService(type));
            return builder;
        }

        public static IStructureAppBuilder AddCache<TCacheManager>(this IStructureAppBuilder builder)
            where TCacheManager : ICacheManager
        {
            var cacheManagerType = typeof(TCacheManager);
            builder.Services.AddSingleton(cacheManagerType);
            builder.Services.AddSingleton(typeof(ICacheManager), provider => provider.GetService(cacheManagerType));
            return builder;
        }

        internal static IStructureAppBuilder AddMessaging(this IStructureAppBuilder builder)
        {
            builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            builder.Services.AddSingleton<IEventBusProcessingContextProvider, UnitOfWorkEventBusProcessingContextProvider>();
            builder.Services.AddScoped<IDistributedEventStore, InMemoryDistributedEventStore>();
            
            return builder;
        }
    }
}
