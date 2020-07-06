using System;
using System.Linq;
using System.Security.Claims;

namespace Structure.Session
{
    public class CurrentUser : ICurrentUser
    {
        private static readonly Claim[] EmptyClaimsArray = new Claim[0];

        public virtual bool IsAuthenticated => Id.HasValue;

        public virtual Guid? Id => this.FindClaimValue<Guid?>(ExtendedClaims.UserId);

        public virtual string UserName => this.FindClaimValue(ExtendedClaims.UserName);

        public virtual string Name => this.FindClaimValue(ExtendedClaims.Name);

        public virtual string PhoneNumber => this.FindClaimValue(ExtendedClaims.PhoneNumber);

        public virtual bool PhoneNumberVerified => string.Equals(this.FindClaimValue(ExtendedClaims.PhoneNumberVerified), "true", StringComparison.InvariantCultureIgnoreCase);

        public virtual string Email => this.FindClaimValue(ExtendedClaims.Email);

        public virtual bool EmailVerified => string.Equals(this.FindClaimValue(ExtendedClaims.Email), "true", StringComparison.InvariantCultureIgnoreCase);

        public virtual Guid? TenantId => this.FindClaimValue<Guid?>(ExtendedClaims.TenantId);

        public virtual string[] Roles => FindClaims(ExtendedClaims.Role).Select(c => c.Value).ToArray();

        private readonly IPrincipalAccessor principalAccessor;

        public CurrentUser(IPrincipalAccessor principalAccessor)
        {
            this.principalAccessor = principalAccessor;
        }

        public virtual Claim FindClaim(string claimType)
        {
            return principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == claimType);
        }

        public virtual Claim[] FindClaims(string claimType)
        {
            return principalAccessor.Principal?.Claims.Where(c => c.Type == claimType).ToArray() ?? EmptyClaimsArray;
        }

        public virtual Claim[] GetAllClaims()
        {
            return principalAccessor.Principal?.Claims.ToArray() ?? EmptyClaimsArray;
        }

        public virtual bool IsInRole(string roleName)
        {
            return FindClaims(ExtendedClaims.Role).Any(c => c.Value == roleName);
        }
    }
}
