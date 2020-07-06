using System.Collections.Generic;

namespace Structure.Security.Authorization
{
    public class AuthorizationRoleOptions
    {
        public IList<StaticRole> StaticRoles { get; }

        public AuthorizationRoleOptions()
        {
            StaticRoles = new List<StaticRole>();
        }
    }
}