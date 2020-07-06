using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Linq.Async
{
    public static class QueryableAsyncExtensions
    {
        public static Task<List<TSource>> ToListAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return AsyncQueryableExecutor.ToListAsync(source, cancellationToken);
        }

        public static Task<TSource> GetFirstOrDefaultAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return AsyncQueryableExecutor.FirstOrDefaultAsync(source, cancellationToken);
        }

        public static Task<TSource> FirstOrDefaultAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return AsyncQueryableExecutor.FirstOrDefaultAsync(source, predicate, cancellationToken);
        }

        public static Task<bool> AnyAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return AsyncQueryableExecutor.AnyAsync(source, cancellationToken);
        }

        public static Task<bool> AnyAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return AsyncQueryableExecutor.AnyAsync(source, predicate, cancellationToken);
        }

        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return AsyncQueryableExecutor.SingleOrDefaultAsync(source, cancellationToken);
        }

        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return AsyncQueryableExecutor.SingleOrDefaultAsync(source, predicate, cancellationToken);
        }
    }
}
