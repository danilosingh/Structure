using Structure.Security.Authorization;
using System.Security.Claims;

namespace Structure.Started.AspNetCore.Authorization
{
    public class JwtAuthenticationResult<TUser> : AuthenticationResult<TUser>
    {
        public ClaimsIdentity ClaimsIdentity { get; set; }
        public string AccessToken { get; set; }
        public int ExpireInSeconds { get; set; }

        public JwtAuthenticationResult()
        { }
    }

    public class RefreshableJwtAuthenticationResult<TUser> : JwtAuthenticationResult<TUser>
    {
        public string RefreshToken { get; set; }

        public RefreshableJwtAuthenticationResult()
        { }
    }
}
