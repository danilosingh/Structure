using System;

namespace Structure.Domain.Entities.Auditing
{
    public interface IHasModificationTime
    {
        DateTime? LastModificationTime { get; set; }
    }
}