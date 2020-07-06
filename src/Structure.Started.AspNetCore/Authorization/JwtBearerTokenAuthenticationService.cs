using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Structure.Extensions;
using Structure.Identity;
using Structure.MultiTenancy;
using Structure.Security.Authorization;
using Structure.Security.Extensions;
using Structure.Session;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Structure.Started.AspNetCore.Authorization
{
    public class JwtBearerTokenAuthenticationService<TUser, TAuthenticationResult> : IAuthenticationService<TUser, TAuthenticationResult>
        where TUser : class
        where TAuthenticationResult : JwtAuthenticationResult<TUser>, new()
    {
        protected readonly IServiceProvider serviceProvider;
        protected readonly UserManager<TUser> userManager;
        protected readonly IUserClaimsPrincipalFactory<TUser> userClaimsPrincipalFactory;
        protected readonly JwtAuthenticationOptions options;

        public JwtBearerTokenAuthenticationService(IServiceProvider serviceProvider,
            UserManager<TUser> userManager,
            IUserClaimsPrincipalFactory<TUser> userClaimsPrincipalFactory,
            IOptions<JwtAuthenticationOptions> options)
        {
            this.serviceProvider = serviceProvider;
            this.userManager = userManager;
            this.userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            this.options = options.Value;
        }

        public virtual async Task<TAuthenticationResult> AuthenticateAsync(string userName, string password, Guid? tenantId = null, bool isPersistent = true, bool lockoutOnFailure = true, params Claim[] additionalClaims)
        {
            var user = await FindUserByNameAsync(userName);

            if (user == null)
            {
                return CreateInvalidResult(AuthenticationResultType.InvalidUserNameOrEmailAddress);
            }

            if (await userManager.IsLockedOutAsync(user))
            {
                return CreateInvalidResult(AuthenticationResultType.LockedOut);
            }

            if (!await userManager.CheckPasswordAsync(user, password))
            {
                return CreateInvalidResult(AuthenticationResultType.InvalidPassword);
            }

            await userManager.ResetAccessFailedCountAsync(user);

            return await CreateSuccessResultAsync(tenantId, user);
        }

        protected virtual async Task<TenantConfiguration> GetTenantConfiguration(Guid? tenantId)
        {
            var tenantStore = serviceProvider.GetService<ITenantStore>();
            return await tenantStore.FindAsync(tenantId.Value);
        }

        //TODO: refactor this method
        public virtual async Task<TAuthenticationResult> ChangeAsync(Guid? tenantId, params Claim[] additionalClaims)
        {
            var currentUser = serviceProvider.GetService<ICurrentUser>();

            if (currentUser == null || !currentUser.IsAuthenticated)
            {
                return CreateInvalidResult(AuthenticationResultType.UnauthenticatedUser);
            }

            if (tenantId.IsNullOrEmpty())
            {
                var tenantInfo = GetTenantConfiguration(tenantId.Value);

                if (tenantInfo == null)
                {
                    return CreateInvalidResult(AuthenticationResultType.InvalidTenancyName);
                }
            }

            var user = await FindUserByNameAsync(currentUser.UserName);

            return await CreateSuccessResultAsync(tenantId, user, additionalClaims);
        }

        protected virtual Task<TUser> FindUserByNameAsync(string userName)
        {
            return userManager.FindByNameAsync(userName);
        }

        protected virtual string CreateAccessToken(ClaimsIdentity claimsIdentity, DateTime notBefore, DateTime expires)
        {
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: claimsIdentity.Claims,
                notBefore: notBefore,
                expires: expires,
                signingCredentials: options.CreateSigningCredentials()
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        protected virtual async Task<TAuthenticationResult> CreateSuccessResultAsync(Guid? tenantId, TUser user, params Claim[] additionalClaims)
        {
            var now = DateTime.UtcNow;
            var expires = now.Add(options.Expiration);
            var principal = await CreatePrincipal(tenantId, user, additionalClaims);
            var identity = principal.GetClaimsIdentity();
            var accessToken = CreateAccessToken(identity, now, expires);

            return await CreateSuccessResultAsync(tenantId, user, identity, accessToken, now, expires, additionalClaims);
        }

        protected virtual TAuthenticationResult CreateInvalidResult(AuthenticationResultType type)
        {
            return new TAuthenticationResult() { Type = type };
        }

        protected virtual Task<TAuthenticationResult> CreateSuccessResultAsync(Guid? tenantId, 
            TUser user,
            ClaimsIdentity identity,
            string accessToken,
            DateTime notBefore,
            DateTime expires,
            Claim[] additionalClaims)
        {
            return Task.FromResult(new TAuthenticationResult()
            {
                Type = AuthenticationResultType.Success,
                TenantId = tenantId,
                User = user,
                AccessToken = accessToken,
                ClaimsIdentity = identity,
                ExpireInSeconds = (int)options.Expiration.TotalSeconds
            });
        }

        protected virtual async Task<ClaimsPrincipal> CreatePrincipal(Guid? tenantId, TUser user, params Claim[] additionalClaims)
        {
            var principal = await userClaimsPrincipalFactory.CreateAsync(user);

            if (!tenantId.IsNullOrEmpty())
            {
                principal.GetClaimsIdentity().AddClaim(ExtendedClaims.TenantId, tenantId);
            }

            IncludeAdditionalClaims(principal.GetClaimsIdentity(), additionalClaims);

            return principal;
        }

        protected virtual void IncludeAdditionalClaims(ClaimsIdentity claimsIdentity, Claim[] additionalClaims)
        {
            foreach (var claim in additionalClaims)
            {
                claimsIdentity.AddClaim(claim);
            }
        }
    }

    public class JwtBearerTokenAuthenticationService<TUser, TRefreshToken, TAuthenticationResult> : JwtBearerTokenAuthenticationService<TUser, TAuthenticationResult>
        where TUser : class
        where TAuthenticationResult : RefreshableJwtAuthenticationResult<TUser>, new()
        where TRefreshToken : IIdentityToken, new()
    {
        private readonly IRefreshTokenStore<TRefreshToken> refreshTokenStore;

        public JwtBearerTokenAuthenticationService(IServiceProvider serviceProvider,
            UserManager<TUser> userManager,
            IUserClaimsPrincipalFactory<TUser> userClaimsPrincipalFactory,
            IRefreshTokenStore<TRefreshToken> refreshTokenStore,
            IOptions<JwtAuthenticationOptions> options) : base(serviceProvider, userManager, userClaimsPrincipalFactory, options)
        {
            this.refreshTokenStore = refreshTokenStore;
        }

        public virtual async Task<TAuthenticationResult> RefreshTokenAsync(string token, params Claim[] additionalClaims)
        {
            var refreshToken = refreshTokenStore.RefreshTokens.FirstOrDefault(c => c.Token == token);

            if (refreshToken == null || !refreshToken.IsActive)
            {
                return CreateInvalidResult(AuthenticationResultType.InvalidRefreshToken);
            }

            var user = await userManager.FindByIdAsync(refreshToken.UserId.ToString());
            var result = await CreateSuccessResultAsync(refreshToken.TenantId, user, additionalClaims);
            
            refreshToken.RevokedAt = DateTime.UtcNow;
            refreshToken.ReplacedByToken = result.RefreshToken;

            await refreshTokenStore.UpdateAsync(refreshToken);

            return result;
        }

        protected override async Task<TAuthenticationResult> CreateSuccessResultAsync(Guid? tenantId, TUser user, ClaimsIdentity identity, string accessToken, DateTime notBefore, DateTime expires, Claim[] additionalClaims)
        {
            var result = await base.CreateSuccessResultAsync(tenantId, user, identity, accessToken, notBefore, expires, additionalClaims);
            var refreshToken = await CreateRefrehTokenAsync(tenantId, user, notBefore, expires);
            result.RefreshToken = refreshToken.Token;
            return result;
        }

        protected virtual async Task<IIdentityToken> CreateRefrehTokenAsync(Guid? tenantId, TUser user, DateTime createdAt, DateTime expires)
        {
            //TODO: create interface to User
            var refreshToken = new TRefreshToken()
            {
                TenantId = tenantId,
                UserId = new Guid(await userManager.GetUserIdAsync(user)),
                Token = Guid.NewGuid().ToString(),
                Expires = expires,
                CreatedAt = createdAt
            };

            await refreshTokenStore.CreateAsync(refreshToken);
            return refreshToken;
        }
    }

    public class JwtBearerTokenAuthenticationService<TUser> : JwtBearerTokenAuthenticationService<TUser, JwtAuthenticationResult<TUser>>
       where TUser : class
    {
        public JwtBearerTokenAuthenticationService(IServiceProvider serviceProvider, UserManager<TUser> userManager, IUserClaimsPrincipalFactory<TUser> userClaimsPrincipalFactory, IOptions<JwtAuthenticationOptions> options) : base(serviceProvider, userManager, userClaimsPrincipalFactory, options)
        { }
    }
}
