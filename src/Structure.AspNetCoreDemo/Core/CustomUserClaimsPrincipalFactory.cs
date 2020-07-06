using Structure.Identity;
using Structure.Tests.Shared.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Structure.AspNetCoreDemo.Core
{
    public class CustomUserClaimsPrincipalFactory : Structure.Identity.UserClaimsPrincipalFactory<User>
    {
        public CustomUserClaimsPrincipalFactory(IIdentityUserManager<User> userManager) : base(userManager)
        { }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var id = await base.GenerateClaimsAsync(user);
            id.AddClaim(new Claim("Teste", "Valor"));
            return id;
        }
    }
}
