using Structure.Helpers;

namespace Structure.Session
{
    public static class CurrentUserExtensions
    {
        public static string FindClaimValue(this ICurrentUser currentUser, string claimType)
        {
            return currentUser.FindClaim(claimType)?.Value;
        }

        public static T FindClaimValue<T>(this ICurrentUser currentUser, string claimType)
        {
            var value = currentUser.FindClaimValue(claimType);

            if (value == null)
            {
                return default;
            }

            return TypeHelper.Convert<T>(value);
        }
    }
}
