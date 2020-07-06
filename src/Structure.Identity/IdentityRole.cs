using Structure.Domain.Entities;
using System;

namespace Structure.Identity
{
    public abstract class IdentityRole : Entity<Guid>
    {
        public virtual string Name { get; set; }
        public virtual string NormalizedName { get; set; }
    }
}
