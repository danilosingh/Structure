using Structure.Application;
using Structure.Validation;
using System;

namespace Structure.Tests.Shared.Dtos
{
    public class UserDto : IEntityDto<Guid>, IShouldNormalize
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public DateTime? DateLastAccess { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }
        public RoleDto Role { get; set; }
        public Guid TenantId { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? LastModifierId { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public void Normalize()
        {
            NormalizedUserName = UserName?.ToUpperInvariant();
        }

        public virtual void Register(string name)
        {
            throw new NotImplementedException();
        }
    }
}
