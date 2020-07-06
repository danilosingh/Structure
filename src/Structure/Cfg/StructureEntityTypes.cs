using System;

namespace Structure.Cfg
{
    public class StructureEntityTypes
    {
        public Type User { get; set; }
        public Type Role { get; set; }
        public Type RolePermission { get; set; }
        public Type UserPermission { get; set; }
        public Type UserRole { get; set; }

        public void EnsureValidTypes()
        { }
    }
}
