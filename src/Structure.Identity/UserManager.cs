using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Structure.Domain.Entities;
using Structure.Extensions;
using Structure.MultiTenancy;
using Structure.Runtime.Caching;
using Structure.Security.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Identity
{
    public class UserManager<TUser, TRole> : UserManager<TUser, EmptyUserPermissionIdentity, TRole>
        where TUser : IdentityUser
        where TRole : IdentityRole
    {
        public UserManager(IUserStore<TUser> store, IRoleManager<TRole> roleManager, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, IPermissionStore permissionStore, ICacheManager cacheManager, ILogger<UserManager<TUser>> logger) : base(store, roleManager, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, permissionStore, cacheManager, logger)
        {
        }
    }

    public class UserManager<TUser, TUserPermission, TRole> : UserManager<TUser>, IIdentityUserManager<TUser>
        where TUser : IdentityUser
        where TRole : IdentityRole
        where TUserPermission : IdentityUserPermission
    {
        private readonly IRoleManager<TRole> roleManager;
        private readonly IPermissionStore permissionStore;
        private readonly ICacheManager cacheManager;

        public bool SupportsUserTenant
        {
            get
            {
                return typeof(TUser).InheritOrImplement<IMultiTenant>();
            }
        }

        public UserManager(IUserStore<TUser> store,
            IRoleManager<TRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            IPermissionStore permissionStore,
            ICacheManager cacheManager,
            ILogger<UserManager<TUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            this.roleManager = roleManager;
            this.permissionStore = permissionStore;
            this.cacheManager = cacheManager;
        }

        public virtual Task<Guid?> GetTenantIdAsync(TUser user)
        {
            if (user is IMultiTenant multiTenant)
            {
                return Task.FromResult(multiTenant.TenantId);
            }

            throw new NotSupportedException("User is not multitenant");
        }
        
        public virtual Task<string> GetNameAsync(TUser user)
        {
            return Task.FromResult(user.Name);
        }

        public async Task<bool> IsGrantedAsync(string tenantId, string userId, string permissionName, CancellationToken cancellationToken, MultiTenancySides? multiTenancySide = null)
        {
            return await IsGrantedAsync(tenantId, userId, permissionStore.GetByName(permissionName), cancellationToken, multiTenancySide);
        }

        public async Task<bool> IsGrantedAsync(string tenantId, string userId, Permission permission, CancellationToken cancellationToken, MultiTenancySides? multiTenancySide = null)
        {
            if (multiTenancySide != null && !permission.MultiTenancySides.HasFlag(multiTenancySide))
            {
                return false;
            }

            var permissionCacheItem = await GetUserPermissionCacheItemAsync(tenantId, userId);

            if (permissionCacheItem == null || permissionCacheItem.ProhibitedPermissions.Contains(permission.Name))
            {
                return false;
            }

            if (permissionCacheItem.GrantedPermissions.Contains(permission.Name))
            {
                return true;
            }

            foreach (var roleId in permissionCacheItem.RoleIds)
            {
                //TODO: check tenant
                if (await roleManager.IsGrantedAsync(roleId, permission.Name, null))
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual async Task<UserPermissionCacheItem> GetUserPermissionCacheItemAsync(string tenantId, string userId)
        {
            var cacheKey = $"{userId}@{(!string.IsNullOrEmpty(tenantId) ? tenantId : "default")}"; ;

            return await cacheManager.GetUserPermissionCache().GetCancelableAsync(cacheKey, async (cancellationToken) =>
            {
                var cacheItem = new UserPermissionCacheItem(userId);
                var user = await Store.FindByIdAsync(userId, cancellationToken);

                if (Store is IUserRoleStore<TUser> userRoleStore)
                {
                    foreach (var roleId in await userRoleStore.GetRolesAsync(user, cancellationToken))
                    {
                        cacheItem.RoleIds.Add(roleId);
                    }
                }

                try
                {
                    if (Store is IUserPermissionStore<TUserPermission> userPermissionStore)
                    {
                        foreach (var userPermission in await userPermissionStore.GetPermissionsAsync(userId, cancellationToken))
                        {
                            if (userPermission.IsGranted)
                            {
                                cacheItem.GrantedPermissions.Add(userPermission.PermissionName);
                            }
                            else
                            {
                                cacheItem.ProhibitedPermissions.Add(userPermission.PermissionName);
                            }
                        }
                    }
                }
                catch
                { }

                return cacheItem;

            });
        }

        public virtual async Task<IList<string>> GetPermissions(TUser user)
        {
            return await GetPermissions(user.Id.ToString());
        }

        public virtual async Task<IList<string>> GetPermissions(string userId)
        {
            //TODO: refactor tenantId param
            // Verify best way to get permissions
            var permissionCacheItem = await GetUserPermissionCacheItemAsync(null, userId);
            return permissionCacheItem.GrantedPermissions.ToList();
        }
    }
}
