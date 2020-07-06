using Structure.MultiTenancy;
using System.Collections.Generic;

namespace Structure.Security.Authorization
{
    public class StaticRole
    {
        public string Id { get; protected set; }
        public string Name { get; protected set; }
        public bool GrantAllByDefault { get; protected set; }
        public IList<string> GrantedPermissions { get; }
        public MultiTenancySides Side { get; protected set; }

        protected StaticRole()
        { }
        public StaticRole(string id, string name, MultiTenancySides side, bool grantAllByDefault = false)
        {
            Id = id;
            Name = name;
            Side = side;
            GrantAllByDefault = grantAllByDefault;
            GrantedPermissions = new List<string>();
        }

        public virtual bool IsGrantedByDefault(Permission permission)
        {
            return GrantAllByDefault || GrantedPermissions.Contains(permission.Name);
        }
    }
}
