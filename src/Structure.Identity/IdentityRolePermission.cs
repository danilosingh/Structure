using Structure.Domain.Entities;
using System;

namespace Structure.Identity
{
    public class IdentityRolePermission : Entity<Guid>
    {
        public virtual Guid RoleId { get; set; }
        public virtual string PermissionName { get; set; }
        public virtual bool IsGranted { get; set; }
    }
}
