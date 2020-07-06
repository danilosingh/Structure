using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Structure.Collections.Extensions;
using Structure.Runtime.Caching;
using Structure.Security.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Identity
{
    public class RoleManager<TRole, TRolePermission> : RoleManager<TRole>, IRoleManager<TRole>
        where TRole : IdentityRole
        where TRolePermission : IdentityRolePermission
    {
        private readonly AuthorizationOptions options;
        private readonly IPermissionStore permissionStore;
        private readonly ICacheManager cacheManager;

        public RoleManager(IRoleStore<TRole> store, IPermissionStore permissionStore, ICacheManager cacheManager, IEnumerable<IRoleValidator<TRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IOptions<AuthorizationOptions> options, ILogger<RoleManager<TRole>> logger)
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {
            this.options = options.Value;
            this.permissionStore = permissionStore;
            this.cacheManager = cacheManager;
        }

        public async Task<List<string>> GetPermissions(string roleId, string tenantId = null, CancellationToken cancellationToken = default)
        {
            var permissionCacheItem = await GetRolePermissionCacheItemAsync(roleId, tenantId);
            return permissionCacheItem.GrantedPermissions.ToList();
        }

        public async Task<bool> IsGrantedAsync(string roleId, string permissionName, string tenantId = null, CancellationToken cancellationToken = default)
        {
            var cacheItem = await GetRolePermissionCacheItemAsync(roleId, tenantId);
            return cacheItem.GrantedPermissions.Contains(permissionName);
        }

        private async Task<RolePermissionCacheItem> GetRolePermissionCacheItemAsync(string roleId, string tenantId)
        {
            var cacheKey = roleId + "@" + (!string.IsNullOrEmpty(tenantId) ? tenantId : "default");

            return await cacheManager.GetRolePermissionCache().GetCancelableAsync(cacheKey, async (cancellationToken) =>
            {
                var cacheItem = new RolePermissionCacheItem(roleId);
                var normalizedRoleId = KeyNormalizer.NormalizeName(roleId);
                var staticRole = options.Role.StaticRoles.FirstOrDefault(c =>
                    string.Equals(c.Id, normalizedRoleId, StringComparison.OrdinalIgnoreCase));

                if (staticRole != null)
                {
                    foreach (var permission in permissionStore.GetAll())
                    {
                        if (staticRole.IsGrantedByDefault(permission))
                        {
                            cacheItem.GrantedPermissions.Add(permission.Name);
                        }
                    }
                }
                else
                {
                    var role = await Store.FindByIdAsync(normalizedRoleId, cancellationToken);

                    if (role == null)
                    {
                        throw new StructureException("There is no role with given id: " + normalizedRoleId);
                    }
                }

                if (Store is IRolePermissionStore<TRolePermission> rolePermissionStore)
                {
                    foreach (var rolePermission in await rolePermissionStore.GetPermissionsAsync(normalizedRoleId, cancellationToken))
                    {
                        if (rolePermission.IsGranted)
                        {
                            cacheItem.GrantedPermissions.AddIfNotContains(rolePermission.PermissionName);
                        }
                        else
                        {
                            cacheItem.GrantedPermissions.Remove(rolePermission.PermissionName);
                        }
                    }
                }

                return cacheItem;
            });
        }
    }
}
