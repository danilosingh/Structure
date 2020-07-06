using System.Security.Claims;

namespace Structure.Session
{
    public interface IPrincipalAccessor
    {
        ClaimsPrincipal Principal { get; }
    }
}
