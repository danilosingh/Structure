using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Data
{
    public interface IDataContext : IDisposable
    {
        T Get<T, TId>(TId id);
        Task<T> GetAsync<T, TId>(TId id);
        IQueryable<T> Query<T>();
        void Create<T>(T entity);
        void Update<T>(T entity);
        void Delete<T>(T entity);
        Task CreateAsync<T>(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync<T>(T entity, CancellationToken cancellationToken = default);
        Task SaveChangesAsync();
    }
}
