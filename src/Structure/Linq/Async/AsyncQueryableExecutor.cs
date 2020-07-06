using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Linq.Async
{
    public static class AsyncQueryableExecutor
    {
        private static IAsyncQueryableExecutor internalExecutor;

        public static Task<List<TSource>> ToListAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return internalExecutor.ToListAsync(source, cancellationToken);
        }

        public static Task<TSource> FirstOrDefaultAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return internalExecutor.FirstOrDefaultAsync(source, cancellationToken);
        }

        public static Task<TSource> FirstOrDefaultAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return internalExecutor.FirstOrDefaultAsync(source, predicate, cancellationToken);
        }

        public static Task<bool> AnyAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return internalExecutor.AnyAsync(source, cancellationToken);
        }

        public static Task<bool> AnyAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return internalExecutor.AnyAsync(source, predicate, cancellationToken);
        }

        public static Task<TSource> SingleOrDefaultAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return internalExecutor.SingleOrDefaultAsync(source, predicate, cancellationToken);
        }

        public static Task<TSource> SingleOrDefaultAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return internalExecutor.SingleOrDefaultAsync(source, cancellationToken);
        }

        public static void SetExecutor(IAsyncQueryableExecutor executor)
        {
            internalExecutor = executor;
        }
    }
}
