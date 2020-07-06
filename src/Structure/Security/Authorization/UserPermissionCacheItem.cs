using System.Collections.Generic;

namespace Structure.Security.Authorization
{
    public class UserPermissionCacheItem
    {
        public string UserId { get; set; }

        public IList<string> RoleIds { get; set; }

        public HashSet<string> GrantedPermissions { get; set; }

        public HashSet<string> ProhibitedPermissions { get; set; }

        public UserPermissionCacheItem()
        {
            RoleIds = new List<string>();
            GrantedPermissions = new HashSet<string>();
            ProhibitedPermissions = new HashSet<string>();
        }

        public UserPermissionCacheItem(string userId)
            : this()
        {
            UserId = userId;
        }
    }
}
