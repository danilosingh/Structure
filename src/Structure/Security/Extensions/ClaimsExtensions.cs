using Structure.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Structure.Security.Extensions
{
    public static class ClaimsExtensions
    {
        public static void AddClaim(this ClaimsIdentity claimsIdentity, string type, object value)
        {
            claimsIdentity.AddClaim(new Claim(type, value.ToString()));
        }

        public static T GetClaim<T>(this ClaimsIdentity claimsIdentity, string type)
        {
            return TypeHelper.Convert<T>(claimsIdentity.FindFirst(type)?.Value);
        }

        public static void RemoveClaim(this ClaimsIdentity claimsIdentity, string type)
        {
            var claim = claimsIdentity.FindFirst(type);
            
            if (claim != null)
            {
                claimsIdentity.RemoveClaim(claim);
            }
        }

        public static void AddSingleClaim(this ClaimsIdentity claimsIdentity, string type, object value)
        {
            claimsIdentity.RemoveAllClaims(type);
            claimsIdentity.AddClaim(type, value);
        }

        public static void RemoveAllClaims(this ClaimsIdentity claimsIdentity, string type)
        {
            var claims = claimsIdentity.FindAll(type);

            foreach (var claim in claims)
            {
                claimsIdentity.RemoveClaim(claim);
            }
        }

        public static bool HasClaim(this ClaimsIdentity claimsIdentity, string type)
        {
            return claimsIdentity.HasClaim(c => c.Type == type);
        }

        public static T GetClaimValue<T>(this ClaimsPrincipal claimsPrincipal, string type)
        {
            return TypeHelper.Convert<T>(claimsPrincipal.GetClaimValue(type));
        }

        public static ClaimsIdentity GetClaimsIdentity(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Identity as ClaimsIdentity;
        }

        public static string GetClaimValue(this ClaimsPrincipal claimsPrincipal, string type)
        {
            return claimsPrincipal.FindFirst(type)?.Value;
        }

        public static IEnumerable<T> GetClaims<T>(this ClaimsPrincipal claimsPrincipal, string type)
        {
            return claimsPrincipal.FindAll(type).Select(c => TypeHelper.Convert<T>(c.Value));
        }

        public static int CountClaims(this ClaimsPrincipal claimsPrincipal, string type)
        {
            return claimsPrincipal.FindAll(type).Count();
        }

        public static bool Contains(this Claim[] claims, string type)
        {
            return claims.Any(c => c.Type == type);
        }
    }
}
