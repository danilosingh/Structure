using Dapper;
using Structure.Collections;
using Structure.Dapper;
using Structure.Data;
using Structure.Data.Repositories;
using Structure.Domain.Queries;
using Structure.Tests.Shared.Domain.Repositories;
using Structure.Tests.Shared.Entities;
using Structure.Extensions;
using System;
using System.Linq;

namespace Structure.AspNetCoreDemo.Repositories
{
    public class UserRepository : Repository<User, Guid>, IUserRepository
    {
        public UserRepository(IDataContext context) : base(context)
        {
        }

        public bool UserIsUnique(Guid id, string userName)
        {          
            return true;
        }

        protected override IPagedList<User> ExecuteQuery(IQueryable<User> query, FilterableQueryInput parameters, int totalCount)
        {
            query = query.Where(c => c.Name.StartsWithIgnoreCaseAndDiacritics(parameters.FilterText));

            return base.ExecuteQuery(query, parameters, totalCount);
        }
    }
}
