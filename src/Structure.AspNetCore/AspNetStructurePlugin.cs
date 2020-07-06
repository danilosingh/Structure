using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Structure.AspNetCore.Extensions;
using Structure.AspNetCore.MultiTenancy;
using Structure.AspNetCore.Mvc.ApplicationModels;
using Structure.AspNetCore.Mvc.Conventions;
using Structure.AspNetCore.Mvc.ExceptionHandling;
using Structure.AspNetCore.Mvc.Uow;
using Structure.AspNetCore.Session;
using Structure.AspNetCore.Validation;
using Structure.Cfg;
using Structure.DependencyInjection;
using Structure.MultiTenancy;
using Structure.Session;
using Structure.Validation.Interception;
using System;

namespace Structure.AspNetCore
{
    public class AspNetStructurePlugin : IStructurePlugin
    {
        private readonly Action<AspNetCoreOptions> setupAction;

        public AspNetStructurePlugin(Action<AspNetCoreOptions> setupAction)
        {
            this.setupAction = setupAction;
        }

        public void Configure(IStructureAppBuilder builder)
        {
            builder.Services.Configure(setupAction);
            builder.Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IPrincipalAccessor, AspNetCorePrincipalAccessor>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IApiControllerSpecification, ApiControllerSpecification>());
            builder.Services.AddScoped<IMethodInvocationValidator, MvcActionInvocationValidator>(); //TODO: check location IMethodInvocationValidator 
            builder.Services.AddScoped<IActionExceptionHandler, ActionExceptionHandler>();            
            
            AddFilters(builder);
            AddConventions(builder);
            AddMultiTenancy(builder);
            ConfigureAddOns(builder);
        }

        private void ConfigureAddOns(IStructureAppBuilder builder)
        {
            //TODO: refactor the way to add addons 
            var opts = new AspNetCoreOptions();
            setupAction?.Invoke(opts);

            foreach (var addOn in opts.AddOns)
            {
                addOn.Configure(builder);
            }
        }

        private void AddMultiTenancy(IStructureAppBuilder builder)
        {
            builder.Services.AddTransient<MultiTenancyMiddleware>();

            builder.Services.Configure<TenantResolveOptions>(options =>
            {
                options.TenantResolvers.Insert(0, new RouteTenantResolveContributor());
                //TODO: Add contributors
                //options.TenantResolvers.Add(new RouteTenantResolveContributor());
                //options.TenantResolvers.Add(new HeaderTenantResolveContributor());
                //options.TenantResolvers.Add(new CookieTenantResolveContributor());
            });
        }

        private void AddFilters(IStructureAppBuilder builder)
        {
            builder.Services.AddScoped(typeof(AuthorizationFilter));
            builder.Services.AddScoped(typeof(ValidationActionFilter));
            builder.Services.AddScoped(typeof(UnitOfWorkActionFilter));
            builder.Services.AddScoped(typeof(ExceptionFilter));

            builder.Services.Configure<MvcOptions>(opts =>
            {
                opts.Filters.AddService(typeof(AuthorizationFilter));
                opts.Filters.AddService(typeof(ValidationActionFilter));
                opts.Filters.AddService(typeof(UnitOfWorkActionFilter));
                opts.Filters.AddService(typeof(ExceptionFilter));
            });
        }

        private void AddConventions(IStructureAppBuilder builder)
        {
            builder.Services.AddSingleton<IRouteModelConvention, DefaultRouteModelConvention>();

            builder.Services.Configure<MvcOptions, IRouteModelConvention>((opts, convention) =>
            {
                opts.Filters.Add(new HttpResponseExceptionFilter());
                opts.Conventions.Add(convention);
            });
        }
    }
}
