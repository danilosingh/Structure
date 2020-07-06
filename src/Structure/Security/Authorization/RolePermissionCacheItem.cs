using System.Collections.Generic;

namespace Structure.Security.Authorization
{
    public class RolePermissionCacheItem
    {
        public const string CacheStoreName = "RolePermissions";

        public string RoleId { get; set; }

        public HashSet<string> GrantedPermissions { get; set; }

        public RolePermissionCacheItem()
        {
            GrantedPermissions = new HashSet<string>();
        }

        public RolePermissionCacheItem(string roleId) : this()
        {
            RoleId = roleId;
        }
    }
}
