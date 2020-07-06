using System;

namespace Structure.Domain.Entities.Auditing
{
    public interface IMayHaveCreator
    {
        Guid? CreatorId { get; set; }
    }
}
