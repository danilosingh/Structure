using System.Security.Claims;

namespace Structure.Session
{
    public class ExtendedClaims
    {
        public const string UserId = ClaimTypes.NameIdentifier;
        public const string UserName = "UserName";
        public const string TenantId = "TenantId";
        public const string UserTenantId = "UserTenantId";
        public const string AuthenticationId = "AuthenticationId";

        public const string Name = "Name";
        public const string PhoneNumber = "PhoneNumber";
        public const string PhoneNumberVerified = "PhoneNumberVerified";
        public const string Email = "Email";
        public const string Role = "Role";
    }
}
