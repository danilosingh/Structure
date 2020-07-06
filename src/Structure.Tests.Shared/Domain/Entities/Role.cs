using Structure.Identity;

namespace Structure.Tests.Shared.Entities
{
    public class Role : IdentityRole
    {
        public virtual RoleComponent RoleComponent { get; set; }
    }
}
