using Structure.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace Structure.Domain.Queries
{
    public static class FilterExtensions
    {
        public static Filter<T> AndId<T>(this Filter<T> filter, object id)
            where T : IEntity
        {
            var parameter = Expression.Parameter(typeof(T));
            var predicate = Expression.Lambda<Func<T, bool>>(
                Expression.Equal(Expression.Property(parameter, "Id"),
                                 Expression.Constant(id)), parameter);

            filter.And(predicate);
            return filter;
        }
        
    }
}
