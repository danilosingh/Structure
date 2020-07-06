using Structure.Domain.Repositories;
using Structure.Tests.Shared.Entities;
using System;

namespace Structure.Tests.Shared.Domain.Repositories
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        bool UserIsUnique(Guid id, string userName);
    }
}
