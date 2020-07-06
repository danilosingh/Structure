using Structure.Domain.Repositories;
using Structure.Tests.Shared.Entities;
using System;

namespace Structure.Tests.Shared.Domain.Repositories
{
    public interface ITopicRepository : IRepository<Topic, Guid>
    {
        void UpdateMaterializedView();
    }
}
