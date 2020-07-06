using Structure.Collections;
using Structure.Domain.Entities;
using Structure.Domain.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Domain.Repositories
{
    public interface IRepository<TEntity, TId> : IRepository<TEntity, TId, FilterableQueryInput>
        where TEntity : IEntity<TId>
    {
    }

    public interface IRepository<TEntity, TId, TQueryInput>
        where TEntity : IEntity<TId>
    {
        TEntity Get(TId id);
        TEntity GetFull(TId id);
        Task<TEntity> GetAsync(TId id);
        Task<TEntity> GetFullAsync(TId id);
        IPagedList<TEntity> GetAll(TQueryInput queryInput);
        Task<IPagedList<TEntity>> GetAllAsync(TQueryInput queryInput);
        Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(TId id);
    }
}