using System;

namespace Structure.Domain.Entities.Auditing
{
    public interface IHasCreationTime
    {
        DateTime CreationTime { get; set; }
    }
}