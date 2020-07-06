using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Structure.Cfg;
using Structure.DependencyInjection;
using Structure.Extensions;
using System;

namespace Structure.Identity
{
    public class IdentityStructurePlugin : IStructurePlugin
    {
        public void Configure(IStructureAppBuilder builder)
        {
            builder.Services.AddScoped(
               typeof(IUserValidator<>).MakeGenericType(builder.EntityTypes.User),
               typeof(UserValidator<>).MakeGenericType(builder.EntityTypes.User));

            builder.Services.AddScoped(
                typeof(IPasswordValidator<>).MakeGenericType(builder.EntityTypes.User),
                typeof(PasswordValidator<>).MakeGenericType(builder.EntityTypes.User));

            builder.Services.AddScoped(
                typeof(IPasswordHasher<>).MakeGenericType(builder.EntityTypes.User),
                typeof(PasswordHasher<>).MakeGenericType(builder.EntityTypes.User));

            builder.Services.AddScoped(
                typeof(IUserClaimsPrincipalFactory<>).MakeGenericType(builder.EntityTypes.User),
                typeof(Identity.UserClaimsPrincipalFactory<>).MakeGenericType(builder.EntityTypes.User));

            builder.Services.AddScoped(
                typeof(IUserConfirmation<>).MakeGenericType(builder.EntityTypes.User),
                typeof(DefaultUserConfirmation<>).MakeGenericType(builder.EntityTypes.User));

            builder.Services.AddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            builder.Services.AddScoped<IdentityErrorDescriber>();

            builder.Services.AddScoped(
                typeof(IRoleValidator<>).MakeGenericType(builder.EntityTypes.Role),
                typeof(RoleValidator<>).MakeGenericType(builder.EntityTypes.Role));


            Type sceUserStoreType;

            if (builder.EntityTypes.UserPermission != null)
            {
                sceUserStoreType = (builder.EntityTypes.UserRole != null ? typeof(UserPermissionStore<,,,>) : typeof(UserPermissionStore<,,>))
                    .MakeGenericTypeNotNullArguments(
                        builder.EntityTypes.User,
                        builder.EntityTypes.UserRole,
                        builder.EntityTypes.UserPermission,
                        builder.EntityTypes.Role
                        );
            }
            else
            {
                sceUserStoreType = (builder.EntityTypes.UserRole != null ? typeof(UserStore<,,>) : typeof(UserStore<,>))
                    .MakeGenericTypeNotNullArguments(
                        builder.EntityTypes.User,
                        builder.EntityTypes.UserRole,
                        builder.EntityTypes.Role
                       );
            }

            var sceUserManagerType = (builder.EntityTypes.UserPermission != null ? typeof(UserManager<,,>) : typeof(UserManager<,>))
                .MakeGenericTypeNotNullArguments(
                    builder.EntityTypes.User,
                    builder.EntityTypes.UserPermission,
                    builder.EntityTypes.Role);

            var roleManagerType = typeof(RoleManager<,>).MakeGenericType(
                    builder.EntityTypes.Role,
                    builder.EntityTypes.RolePermission);

            builder.Services.AddScoped(typeof(IUserStore<>).MakeGenericType(builder.EntityTypes.User), sceUserStoreType);
            builder.Services.AddScoped(typeof(IIdentityUserManager<>).MakeGenericType(builder.EntityTypes.User), sceUserManagerType);
            builder.Services.AddScoped(typeof(UserManager<>).MakeGenericType(builder.EntityTypes.User), sceUserManagerType);

            builder.Services.AddScoped(new Type[] {
                typeof(IRoleManager<>).MakeGenericType(builder.EntityTypes.Role),
                typeof(RoleManager<>).MakeGenericType(builder.EntityTypes.Role)
                }, roleManagerType);

            if (builder.EntityTypes.RolePermission != null)
            {
                builder.Services.AddScoped(typeof(IRoleStore<>).MakeGenericType(builder.EntityTypes.Role),
                    typeof(RoleStore<,>).MakeGenericType(
                        builder.EntityTypes.Role,
                        builder.EntityTypes.RolePermission));
            }
            else
            {
                builder.Services.AddScoped(typeof(IRoleStore<>).MakeGenericType(builder.EntityTypes.Role),
                typeof(RoleStore<>).MakeGenericTypeNotNullArguments(
                    builder.EntityTypes.Role));
            }
        }
    }
}
