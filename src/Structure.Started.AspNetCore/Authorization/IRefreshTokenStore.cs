using Structure.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Structure.Started.AspNetCore.Authorization
{
    public interface IRefreshTokenStore<TRefreshToken>
        where TRefreshToken : IIdentityToken
    {
        IQueryable<TRefreshToken> RefreshTokens { get; }
        Task CreateAsync(TRefreshToken refreshToken);
        Task RemoveAsync(string token);
        Task UpdateAsync(TRefreshToken refreshToken);
    }
}
