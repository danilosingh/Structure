using System;

namespace Structure.Identity
{
    public interface IIdentityToken
    {
        Guid? TenantId { get; set; }
        Guid? UserId { get; set; }
        string Token { get; set; }
        DateTime Expires { get; set; }
        bool IsExpired { get; }
        DateTime CreatedAt { get; set; }
        DateTime? RevokedAt { get; set; }
        bool IsActive { get; }
        string ReplacedByToken { get; set; }
    }
}
