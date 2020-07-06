using Structure.Application;
using Structure.Tests.Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Structure.Tests.Shared.Dtos
{
    public class TopicDto : IEntityDto<Guid>
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public IList<AnotherChildDto> Children { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual Guid? LastModifierId { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual Guid CreatorId { get; set; }

        public TopicDto()
        {
            Children = new List<AnotherChildDto>();
        }
    }
}
