using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Structure.Application;
using Structure.AspNetCore;
using Structure.AspNetCore.Extensions;
using Structure.AspNetCore.MultiTenancy;
using Structure.AspNetCoreDemo.Core;
using Structure.AspNetCoreDemo.Events.Integration;
using Structure.AutoMapper;
using Structure.Cfg;
using Structure.DependencyInjection;
using Structure.Identity;
using Structure.Messaging.EventBus.Abstractions;
using Structure.MultiTenancy;
using Structure.Nhibernate;
using Structure.RabbitMQ;
using Structure.Security.Authorization;
using Structure.Started.AspNetCore;
using Structure.Started.AspNetCore.Authorization;
using Structure.Tests.Shared.Entities;
using System;
using System.Linq;

namespace Structure.AspNetCoreDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtOptions = services.AddSingletonOption<JwtAuthenticationOptions>(Configuration, "Authentication:JwtBearer");

            services.AddControllers()
                 .ConfigureApiBehaviorOptions(opts =>
                 {
                     opts.InvalidModelStateResponseFactory = DefaultInvalidModelStateResponse;
                     opts.SuppressModelStateInvalidFilter = true;
                     opts.ClientErrorMapping[404].Link = "https://httpstatuses.com/404";
                 });

            services.AddStructure(Configuration, (builder) =>
             {
                 builder.EntityTypes
                     .Role<Role, RolePermission>()
                     .User<User, UserRole>();

                 builder.Plugins.AddAspNetCore(opts =>
                 {
                     opts.Routes.UseRouteVersioning = true;
                     opts.Routes.UseApiPrefix = true;
                     opts.Routes.UseKebapCase = true;
                     opts.AddOns.Add(new JwtAuthorizationAspNetAddOn(jwtOptions));
                     opts.AddOns.Add(new AspNetStartedAddOn());
                 })
                 .AddIdentity()
                 .AddNhibernate()
                 .AddAutoMapper(assemblies =>
                 {
                     assemblies.Add("Structure.AspNetCoreDemo");
                 })
                 .AddRabbitMQ(c =>
                 {
                     c.BrokerName = "default";
                     c.QueueName = "structure-queue";
                 });

                 builder.Modules.Add<TestModule>();
             });

            services.AddOrReplace(typeof(IUserClaimsPrincipalFactory<User>), typeof(CustomUserClaimsPrincipalFactory), ServiceLifetime.Scoped);
            services.AddOrReplace(typeof(IPersistenceConfigurerProvider), typeof(CustomPersistenceConfigurerProvider), ServiceLifetime.Singleton);
            services.AddOrReplace(typeof(ISessionFactoryBuilder), typeof(CustomSessionFactoryBuilder), ServiceLifetime.Singleton);

            services.Configure<AuthorizationOptions>(options =>
            {
                options.Role.StaticRoles.Add(new StaticRole("1", "Admin", MultiTenancy.MultiTenancySides.Host, true));
            });

            services.Configure<MultiTenancyOptions>((options) =>
            {
                options.IsEnabled = true;
            });

            services.Configure<UnitOfWorkOptions>(options =>
            {
                options.PublishIntegrationEventsOnComplete = true;
            });

            services.Configure<RouteTenantResolverOptions>(options =>
            {
                options.HostValue = "a";
            });

            services.Configure<DefaultTenantStoreOptions>(options =>
            {
                options.Tenants = new[]
                {
                    new TenantConfiguration(new Guid("9dca757c-1d2b-4f9f-b748-7615bbdcd979"), "Tenant 1"),
                    new TenantConfiguration(new Guid("21604440-dfa6-4e26-9a87-ea5a62b9c6c8"), "Tenant 2")
                };
            });

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }

        private static IActionResult DefaultInvalidModelStateResponse(ActionContext context)
        {
            var result = new BadRequestObjectResult(context.ModelState.Select(c => c.Value));
            result.ContentTypes.Add("application/json");
            return result;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStructure();
            app.UseEndpoints(endpoints =>
            {
                //" { language}/{ controller = Home}/{ action = Index}/{ id ?}"
                endpoints.MapControllerRoute("default", "api/v{v:apiVersion}/{company}/{tenant}/{controller}/{action}/{id?}");
            });
            ConfigureEventBusSubscriptions(app);
        }

        private void ConfigureEventBusSubscriptions(IApplicationBuilder app)
        {
            var subscriptionManager = app.ApplicationServices.GetService<IEventBus>();
            subscriptionManager.Subscribe<UserRegistered, UserRegisteredIntegrationEventHandler>();
        }
    }
}
