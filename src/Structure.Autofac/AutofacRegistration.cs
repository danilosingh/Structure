using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Structure.Autofac
{
    public static class AutofacRegistration
    {
        private static IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> ConfigureLifecycle<TActivatorData, TRegistrationStyle>(
             this IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> registrationBuilder,
             ServiceLifetime lifecycleKind)
        {
            switch (lifecycleKind)
            {
                case ServiceLifetime.Singleton:
                    registrationBuilder.SingleInstance();
                    break;
                case ServiceLifetime.Scoped:
                    registrationBuilder.InstancePerLifetimeScope();
                    break;
                case ServiceLifetime.Transient:
                    registrationBuilder.InstancePerDependency();
                    break;
            }

            return registrationBuilder;
        }

        public static void Register(
                ContainerBuilder builder,
                IServiceCollection services)
        {
            foreach (var service in services)
            {
                if (service.ImplementationType != null)
                {
                    var serviceTypeInfo = service.ServiceType.GetTypeInfo();
                    if (serviceTypeInfo.IsGenericTypeDefinition)
                    {
                        builder
                            .RegisterGeneric(service.ImplementationType)
                            .As(service.ServiceType)
                            .ConfigureLifecycle(service.Lifetime)
                            .ConfigureConventions();
                    }
                    else
                    {
                        builder
                            .RegisterType(service.ImplementationType)
                            .As(service.ServiceType)
                            .ConfigureLifecycle(service.Lifetime)
                            .ConfigureConventions();
                    }
                }
                else if (service.ImplementationFactory != null)
                {
                    var registration = RegistrationBuilder.ForDelegate(service.ServiceType, (context, parameters) =>
                    {
                        var serviceProvider = context.Resolve<IServiceProvider>();
                        return service.ImplementationFactory(serviceProvider);
                    })
                    .ConfigureLifecycle(service.Lifetime)
                    .CreateRegistration();

                    builder.RegisterComponent(registration);
                }
                else
                {
                    builder
                        .RegisterInstance(service.ImplementationInstance)
                        .As(service.ServiceType)
                        .ConfigureLifecycle(service.Lifetime);
                }
            }
        }

        private static IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> ConfigureConventions<TActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> registrationBuilder)
        {            
            return registrationBuilder.PropertiesAutowired();
        }
    }
}
