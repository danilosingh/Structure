using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Linq.Async
{
    public interface IAsyncQueryableExecutor
    {
        Task<List<TSource>> ToListAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default);
        Task<TSource> FirstOrDefaultAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default);
        Task<TSource> FirstOrDefaultAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default);
        Task<TSource> SingleOrDefaultAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default);
        Task<TSource> SingleOrDefaultAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
