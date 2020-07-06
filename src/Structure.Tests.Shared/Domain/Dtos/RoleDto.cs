using Structure.Application;
using System;

namespace Structure.Tests.Shared.Dtos
{
    public class RoleDto : IEntityDto<Guid>
    {
        public Guid Id { get; set;}
        public string Name { get; set; }
    }
}