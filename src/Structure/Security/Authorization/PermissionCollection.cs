using Structure.Collections;
using Structure.Extensions;
using Structure.Localization;
using Structure.MultiTenancy;
using System.Collections.Generic;
using System.Linq;

namespace Structure.Security.Authorization
{
    internal class PermissionCollection : IPermissionCollection, IHierarchicalCollection
    {
        private readonly HierarchicalDictionary<string, Permission> permissions;

        public Permission this[string name]
        {
            get { return permissions[name]; }
        }

        public PermissionCollection()
        {
            permissions = new HierarchicalDictionary<string, Permission>();
        }

        public Permission Add(string name, ILocalizableString displayName = null, ILocalizableString description = null, MultiTenancySides multiTenancySides = MultiTenancySides.Tenant | MultiTenancySides.Host)
        {
            var permission = new Permission(name, displayName, displayName, multiTenancySides);
            permissions.Add(name, permission); 
            return permission;
        }

        public Permission Get(string name)
        {
            return permissions.GetOrDefault(name);
        }

        public IEnumerable<Permission> All()
        {
            return permissions.Values.ToList();
        }
        public void Flatten()
        {
            permissions.Flatten();
        }
    }
}
