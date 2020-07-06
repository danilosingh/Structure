using Microsoft.Extensions.Options;
using Structure.Helpers;
using Structure.MultiTenancy;
using Structure.Security.Extensions;
using System;
using System.Security.Claims;

namespace Structure.Session
{
    public class ClaimsAppSession : AppSessionBase
    {
        private readonly IPrincipalAccessor principalAcessor;

        protected ClaimsPrincipal Principal
        {
            get { return principalAcessor.Principal as ClaimsPrincipal; }
        }

        public override Guid? TenantId
        {
            get
            {
                var tenantIdClaimValue = Principal?.GetClaimValue(ExtendedClaims.TenantId);
                return TypeHelper.Convert<Guid?>(tenantIdClaimValue);
            }
        }

        public override Guid? UserId
        {
            get { return TypeHelper.Convert<Guid?>(Principal?.GetClaimValue(ExtendedClaims.UserId)); }
        }

        public ClaimsAppSession(IPrincipalAccessor principalAcessor, IOptions<MultiTenancyOptions> multiTenancy) : base(multiTenancy)
        {
            this.principalAcessor = principalAcessor;
        }
    }
}
