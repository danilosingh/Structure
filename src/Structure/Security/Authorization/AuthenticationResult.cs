using System;

namespace Structure.Security.Authorization
{
    public class AuthenticationResult<TUser>
    {
        public Guid? TenantId { get; set; }
        public TUser User { get; set; }
        public AuthenticationResultType Type { get; set; }

        public AuthenticationResult(Guid? tenantId, TUser user, AuthenticationResultType type)
        {
            TenantId = tenantId;
            User = user;
            Type = type;
        }

        public AuthenticationResult()
        { }
    }
}
