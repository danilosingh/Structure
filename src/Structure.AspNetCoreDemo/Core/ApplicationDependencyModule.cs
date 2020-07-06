using Autofac;
using Structure.AspNetCoreDemo.Application;
using Structure.AspNetCoreDemo.Repositories;
using Structure.Security.Authorization;
using Structure.Tests.Shared.Domain.Repositories;
using Structure.Tests.Shared.Domain.Validators;
using Module = Autofac.Module;

namespace Structure.AspNetCoreDemo.Core
{
    public class ApplicationDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PermissionRegistration>().As<IPermissionRegistration>().InstancePerLifetimeScope();

            #region Application 

            builder.RegisterType<UserAppService>().As<IUserAppService>().InstancePerLifetimeScope();
            builder.RegisterType<TopicAppService>().As<ITopicAppService>().InstancePerLifetimeScope();

            #endregion

            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TopicRepository>().As<ITopicRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SaveUserValidator>().As<SaveUserValidator>().InstancePerLifetimeScope();
        }
    }
}
