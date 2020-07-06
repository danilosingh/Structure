using Microsoft.AspNetCore.Identity;
using Structure.Session;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Structure.Identity
{
    public class UserClaimsPrincipalFactory<TUser> : IUserClaimsPrincipalFactory<TUser>
        where TUser : class
    {
        private readonly IIdentityUserManager<TUser> userManager;

        public UserClaimsPrincipalFactory(IIdentityUserManager<TUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<ClaimsPrincipal> CreateAsync(TUser user)
        {
            var id = await GenerateClaimsAsync(user);
            return new ClaimsPrincipal(id);
        }

        protected virtual async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
        {            
            var id = new ClaimsIdentity("Identity.Application");
            id.AddClaim(new Claim(ExtendedClaims.UserId, await userManager.GetUserIdAsync(user)));
            id.AddClaim(new Claim(ExtendedClaims.UserName, await userManager.GetUserNameAsync(user)));
            id.AddClaim(new Claim(ExtendedClaims.Name, await userManager.GetNameAsync(user)));

            if (userManager.SupportsUserTenant)
            {
                var tenantId = await userManager.GetTenantIdAsync(user);

                if (tenantId != null)
                {
                    id.AddClaim(new Claim(ExtendedClaims.UserTenantId, tenantId?.ToString()));
                }
            }

            if (userManager.SupportsUserEmail)
            {
                id.AddClaim(new Claim(ExtendedClaims.Email, await userManager.GetEmailAsync(user)));
            }

            if (userManager.SupportsUserPhoneNumber)
            {
                id.AddClaim(new Claim(ExtendedClaims.PhoneNumber, await userManager.GetPhoneNumberAsync(user)));
            }

            if (userManager.SupportsUserRole)
            {
                foreach (var roleName in await userManager.GetRolesAsync(user))
                {
                    id.AddClaim(new Claim(ExtendedClaims.Role, roleName));
                }
            }

            return id;
        }
    }
}
