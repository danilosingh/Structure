using Structure.Security.Authorization;

namespace Structure.AspNetCore.Authentication
{
    public class AspNetAuthorizationOptions : AuthorizationOptions
    {
        public bool UseConventionedPermissions { get; set; }

        public AspNetAuthorizationOptions()
        { }
    }
}
