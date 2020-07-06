using Structure.Collections;
using Structure.Collections.Extensions;
using Structure.Domain.Entities;
using Structure.Domain.Entities.Auditing;
using Structure.Domain.Queries;
using Structure.Domain.Repositories;
using Structure.Extensions;
using Structure.Linq;
using Structure.Linq.Async;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Data.Repositories
{
    public abstract class RepositoryBase
    {
        protected readonly IDataContext context;

        protected RepositoryBase(IDataContext context)
        {
            this.context = context;
        }
    }

    public class Repository<TEntity, TId> : Repository<TEntity, TId, FilterableQueryInput>, IRepository<TEntity, TId>
       where TEntity : class, IEntity<TId>
    {
        public Repository(IDataContext context) : base(context)
        { }
    }

    public class Repository<TEntity, TId, TQueryInput> : RepositoryBase, IRepository<TEntity, TId, TQueryInput>
        where TEntity : class, IEntity<TId>
    {
        public Repository(IDataContext context) : base(context)
        { }

        public virtual async Task<TEntity> GetAsync(TId id)
        {
            return await context.GetAsync<TEntity, TId>(id);
        }

        public virtual async Task<TEntity> GetFullAsync(TId id)
        {
            return await GetAsync(id);
        }

        public virtual async Task<IPagedList<TEntity>> GetAllAsync(TQueryInput parameters)
        {
            var query = CreateQuery(parameters);
            var totalCount = query.Count();

            query = ApplySorting(query, parameters);
            query = ApplyPaging(query, parameters);
            query = ApplyIncludes(query, parameters);

            return await ExecuteQueryAsync(query, totalCount);
        }

        public virtual TEntity Get(TId id)
        {
            return context.Get<TEntity, TId>(id);
        }

        public virtual TEntity GetFull(TId id)
        {
            return Get(id);
        }

        public virtual IPagedList<TEntity> GetAll(TQueryInput parameters)
        {
            var query = CreateQuery(parameters);
            var totalCount = query.Count();

            query = ApplySorting(query, parameters);
            query = ApplyPaging(query, parameters);
            query = ApplyIncludes(query, parameters);

            return ExecuteQuery(query, parameters, totalCount);
        }

        protected virtual IPagedList<TEntity> ExecuteQuery(IQueryable<TEntity> query, TQueryInput parameters, int totalCount)
        {
            return query.ToPagedList(totalCount);
        }

        protected async virtual Task<IPagedList<TEntity>> ExecuteQueryAsync(IQueryable<TEntity> query, int totalCount)
        {
            var results = await query.ToListAsync();
            return results.ToPagedList(totalCount);
        }

        protected virtual IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, TQueryInput parameters)
        {
            return query;
        }

        protected virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TQueryInput parameters)
        {
            if (parameters is ISortedQueryInput sortedParameters)
            {
                return query.SortedBy(sortedParameters, typeof(TEntity).InheritOrImplement<IHasCreationTime>() ? "CreationTime DESC, Id DESC" : "Id DESC");
            }

            return query;
        }

        protected virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TQueryInput parameters)
        {
            if (parameters is IPagedQueryInput pagedParameters)
            {
                return query.PagedBy(pagedParameters);
            }

            return query;
        }

        protected virtual IQueryable<TEntity> CreateQuery(TQueryInput parameters)
        {
            var query = context.Query<TEntity>();

            var filterExpression = CreateFilterExpression(parameters);

            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            return query;
        }

        protected virtual Expression<Func<TEntity, bool>> CreateFilterExpression(TQueryInput parameters)
        {          
            return null;
        }

        public virtual void Update(TEntity entity)
        {
            context.Update(entity);
        }

        public async virtual Task DeleteAsync(TEntity entity)
        {
            await context.DeleteAsync(entity);
        }

        public async virtual Task DeleteAsync(TId id)
        {
            await DeleteAsync(Get(id));
        }

        public virtual async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await context.CreateAsync(entity, cancellationToken);
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await context.UpdateAsync(entity, cancellationToken);
        }
    }
}
