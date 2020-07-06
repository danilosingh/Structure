using Structure.Identity;
using Structure.Nhibernate.Mapping;
using Structure.Started.AspNetCore.Authorization;

namespace Structure.AspNetCoreDemo.Core.Mappings
{
    public class RefreshTokenMap : EntityClassMap<IdentityToken>
    {
        public RefreshTokenMap()
        {
            Id(c => c.Token).GeneratedBy.Assigned();
            Map(c => c.CreatedAt);
            Map(c => c.RevokedAt);
            Map(c => c.Expires);
            Map(c => c.ReplacedByToken);
            Map(c => c.UserId);
            Map(c => c.TenantId);
        }
    }
}
