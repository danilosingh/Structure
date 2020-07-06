using NHibernate.Linq;
using Structure.Linq.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Nhibernate.Linq
{
    public class NhAsyncQueryableExecutor : IAsyncQueryableExecutor
    {
        public Task<List<TSource>> ToListAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {            
            return LinqExtensionMethods.ToListAsync(source, cancellationToken);
        }

        public Task<TSource> FirstOrDefaultAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return LinqExtensionMethods.FirstOrDefaultAsync(source, cancellationToken);
        }

        public Task<TSource> FirstOrDefaultAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return LinqExtensionMethods.FirstOrDefaultAsync(source, predicate, cancellationToken);
        }
        
        public Task<bool> AnyAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return LinqExtensionMethods.AnyAsync(source, predicate, cancellationToken);
        }

        public Task<bool> AnyAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return LinqExtensionMethods.AnyAsync(source, cancellationToken);
        }

        public Task<TSource> SingleOrDefaultAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return LinqExtensionMethods.SingleOrDefaultAsync(source, predicate, cancellationToken);
        }

        public Task<TSource> SingleOrDefaultAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return LinqExtensionMethods.SingleOrDefaultAsync(source, cancellationToken);
        }
    }
}
