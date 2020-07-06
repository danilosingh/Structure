using System;

namespace Structure.Domain.Entities.Auditing
{
    public interface IMustHaveCreator
    {
        Guid CreatorId { get; set; }
    }
}
