using System;

namespace Structure.Domain.Entities.Auditing
{
    public interface IModificationAudited : IHasModificationTime
    {
        Guid? LastModifierId { get; set; }
    }
}