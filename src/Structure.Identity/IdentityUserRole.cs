using Structure.Domain.Entities;
using System;

namespace Structure.Identity
{
    public class IdentityUserRole<TUser, TRole> : Entity<Guid> 
    {
        public virtual TUser User { get; set; }
        public virtual TRole Role { get; set; }
    }
}
