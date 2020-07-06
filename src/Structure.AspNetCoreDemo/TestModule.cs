using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Structure.AspNetCoreDemo.Application;
using Structure.AspNetCoreDemo.Core;
using Structure.AspNetCoreDemo.Events;
using Structure.AspNetCoreDemo.Events.Integration;
using Structure.AspNetCoreDemo.Repositories;
using Structure.Cfg;
using Structure.DependencyInjection;
using Structure.Domain.Events;
using Structure.Identity;
using Structure.Messaging.EventBus.Abstractions;
using Structure.Security.Authorization;
using Structure.Started.AspNetCore.Authorization;
using Structure.Tests.Shared.Domain.Repositories;
using Structure.Tests.Shared.Domain.Validators;
using Structure.Tests.Shared.Entities;

namespace Structure.AspNetCoreDemo
{
    public class TestModule : IStructureModule
    {
        public void Configure()
        { }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPermissionRegistration, PermissionRegistration>();
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<ITopicAppService, TopicAppService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<SaveUserValidator>();
            services.AddScoped<UserSettings>();
            services.AddOrReplace(typeof(IUserClaimsPrincipalFactory<User>), typeof(CustomUserClaimsPrincipalFactory), ServiceLifetime.Scoped);
            services.AddScoped(typeof(JwtBearerTokenAuthenticationService<User, IdentityToken, RefreshableJwtAuthenticationResult<User>>));
            services.AddScoped<IDomainEventHandler<UserRegisterDomainEvent>, UserRegisterDomainEventHandler>();
            services.AddScoped<IIntegrationEventHandler<UserRegistered>, UserRegisteredIntegrationEventHandler>();
            services.AddScoped<UserRegisteredIntegrationEventHandler>();
        }
    }
}
