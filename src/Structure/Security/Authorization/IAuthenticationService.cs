using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Structure.Security.Authorization
{
    public interface IAuthenticationService<TUser, TAuthenticationResult>
        where TAuthenticationResult : AuthenticationResult<TUser>
    {
        Task<TAuthenticationResult> AuthenticateAsync(string userName, string password, Guid? tenantId = null, bool isPersistent = true, bool lockoutOnFailure = true, params Claim[] additionalClaims);
        Task<TAuthenticationResult> ChangeAsync(Guid? tenantId, params Claim[] additionalClaims);
    }
}
