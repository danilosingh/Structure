using Structure.Runtime.Caching;

namespace Structure.Security.Authorization
{
    public static class AuthorizationCacheManagerExtensions
    {
        public static ITypedCache<string, UserPermissionCacheItem> GetUserPermissionCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, UserPermissionCacheItem>("UserPermissions");
        }

        public static ITypedCache<string, RolePermissionCacheItem> GetRolePermissionCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, RolePermissionCacheItem>(RolePermissionCacheItem.CacheStoreName);
        }
    }
}
