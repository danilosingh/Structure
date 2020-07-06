using System.Security.Claims;
using System.Threading.Tasks;

namespace Structure.Security.Authorization
{
    public interface IClaimsPrincipalFactory<TUser>
    {
        Task<ClaimsPrincipal> CreateAsync(TUser user);
    }
}
