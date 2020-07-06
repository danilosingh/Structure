using System;

namespace Structure.Domain.Entities.Auditing
{
    public interface ICreationAudited : IHasCreationTime, IMustHaveCreator
    {
    }
}