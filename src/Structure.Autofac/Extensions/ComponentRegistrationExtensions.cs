using Autofac;
using Autofac.Core;
using Structure.Extensions;
using System;
using System.Linq;

namespace Structure.Autofac.Extensions
{
    public static class ComponentRegistrationExtensions
    {
        public static bool HasServiceType<T>(this IComponentRegistration componentRegistration)
        {
            return componentRegistration.Services.OfType<TypedService>().Any(c => c.ServiceType.InheritOrImplement(typeof(T)));
        }

        public static void OnActivatedForInstance<T>(this IComponentRegistration componentRegistration, Action<T, IComponentContext> action, bool validTargetTypeBeforeAttach = true)
        {
            if (validTargetTypeBeforeAttach && !componentRegistration.Target.Activator.LimitType.InheritOrImplement(typeof(T)))
            {
                return;
            }

            componentRegistration.Activated += (sender, eventArgs) =>
            {
                if (eventArgs.Instance is T obj)
                {
                    action(obj, eventArgs.Context);
                }
            };
        }
    }
}
