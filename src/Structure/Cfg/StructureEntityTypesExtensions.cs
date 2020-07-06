using Structure.Domain.Entities;
using System;

namespace Structure.Cfg
{
    public static class StructureEntityTypesExtensions
    {
        public static StructureEntityTypes User<TUser>(this StructureEntityTypes entityTypes)
        {
            entityTypes.User = typeof(TUser);
            return entityTypes;
        }

        public static StructureEntityTypes User<TUser, TUserRole>(this StructureEntityTypes entityTypes)
        {
            entityTypes.User<TUser>();
            entityTypes.UserRole = typeof(TUserRole);
            return entityTypes;
        }

        public static StructureEntityTypes User<TUser, TUserPermission, TUserRole>(this StructureEntityTypes entityTypes)
        {
            entityTypes.User<TUser, TUserRole>();
            entityTypes.UserPermission = typeof(TUserPermission);
            return entityTypes;
        }

        public static StructureEntityTypes Role<TRole, TRolePermission>(this StructureEntityTypes entityTypes)
        {
            entityTypes.Role = typeof(TRole);
            entityTypes.RolePermission = typeof(TRolePermission);
            return entityTypes;
        }
    }

    public class FakeTenant : Entity<Guid>
    { }
}
