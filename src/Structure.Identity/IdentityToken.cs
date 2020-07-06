using System;

namespace Structure.Identity
{
    public class IdentityToken : IIdentityToken
    {
        public virtual Guid? TenantId { get; set; }
        public virtual Guid? UserId { get; set; }
        public virtual string Token { get; set; }
        public virtual DateTime Expires { get; set; }
        public virtual bool IsExpired => DateTime.UtcNow >= Expires;
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? RevokedAt { get; set; }
        public virtual bool IsActive => RevokedAt == null && !IsExpired;
        public virtual string ReplacedByToken { get; set; }

        public IdentityToken(Guid? tenantId, Guid userId, string token, DateTime expires, DateTime createdAt)
        {
            TenantId = tenantId;
            UserId = userId;
            Expires = expires;
            Token = token;
            CreatedAt = createdAt;
        }

        public IdentityToken()
        { }
    }
}
