using Structure.Session;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Structure.AspNetCore.Session
{
    public class AspNetCorePrincipalAccessor : IPrincipalAccessor
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ClaimsPrincipal Principal
        {
            get { return httpContextAccessor.HttpContext.User; }
        }

        public AspNetCorePrincipalAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
    }
}
