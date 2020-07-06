using Structure.Localization;
using Structure.MultiTenancy;
using System.Collections.Generic;

namespace Structure.Security.Authorization
{
    public interface IPermissionCollection
    {
        Permission Add(string name, ILocalizableString displayName = null, ILocalizableString description = null,
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant);

        Permission Get(string name);

        IEnumerable<Permission> All();        
    }
}
