using Structure.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Structure.Domain
{
    public class Filter<T>
    {
        public Expression<Func<T, bool>> Expression { get; private set; }

        public Filter()
        { }

        public Filter(Expression<Func<T, bool>> expression)
        {
            Expression = expression;
        }

        public Filter<T> And(Filter<T> filter)
        {
            Expression = Expression != null ? Expression.And(filter.Expression) : filter.Expression;
            return this;
        }

        public Filter<T> Or(Filter<T> filter)
        {
            Expression = Expression != null ? Expression.Or(filter.Expression) : filter.Expression;
            return this;
        }

        public void And(Expression<Func<T, bool>> expression)
        {
            Expression = Expression != null ? Expression.And(expression) : expression;
        }

        public void Or(Expression<Func<T, bool>> expression)
        {
            Expression = Expression != null ? Expression.Or(expression) : expression;
        }

        public bool HasExpression()
        {
            return Expression != null;
        }

        public static Filter<T> Create(Expression<Func<T, bool>> expression)
        {
            return new Filter<T>()
            {
                Expression = expression
            };
        }

        public static bool IsValid(Filter<T> filter)
        {
            return filter != null && filter.Expression != null;
        }

        public IQueryable<T> ApplyFilter(IQueryable<T> query)
        {
            return Expression != null ? query.Where(Expression) : query;
        }
    }
}
