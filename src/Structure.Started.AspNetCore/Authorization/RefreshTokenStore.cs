using Structure.Data;
using Structure.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Structure.Started.AspNetCore.Authorization
{
    public class RefreshTokenStore<TRefreshToken> : IRefreshTokenStore<TRefreshToken>
        where TRefreshToken : IIdentityToken
    {
        private readonly IDataContext dataContext;

        public IQueryable<TRefreshToken> RefreshTokens
        {
            get
            {
                return dataContext.Query<TRefreshToken>();
            }
        }

        public Task CreateAsync(TRefreshToken refreshToken)
        {
            return dataContext.CreateAsync(refreshToken);
        }

        public Task RemoveAsync(string token)
        {
            var refreshToken = RefreshTokens.FirstOrDefault(c => c.Token == token);
            return dataContext.DeleteAsync(refreshToken);
        }

        public Task UpdateAsync(TRefreshToken refreshToken)
        {
            return dataContext.UpdateAsync(refreshToken);
        }

        public RefreshTokenStore(IDataContext dataContext)
        {
            this.dataContext = dataContext;
        }
    }
}
