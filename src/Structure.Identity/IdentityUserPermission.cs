using Structure.Domain.Entities;
using System;

namespace Structure.Identity
{
    public abstract class IdentityUserPermission : Entity<Guid>
    {
        public virtual Guid UserId { get; set; }
        public virtual string PermissionName { get; set; }
        public virtual bool IsGranted { get; set; }
    }
}
