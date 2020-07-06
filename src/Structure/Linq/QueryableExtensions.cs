using Structure.Domain;
using Structure.Domain.Queries;
using Structure.Extensions;
using Structure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Linq
{
    public static class QueryableExtensions
    {        
        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, Order<TSource> order)
        {
            if (order == null || order.Count == 0)
            {
                return source;
            }

            IOrderedQueryable<TSource> orderedQueryable = order[0].Type == OrderType.Asc ?
                source.OrderBy(order[0].MemberExpression) :
                source.OrderByDescending(order[0].MemberExpression);

            for (int i = 1; i < order.Count; i++)
            {
                orderedQueryable = order[i].Type == OrderType.Asc ?
                    orderedQueryable.ThenBy(order[i].MemberExpression) :
                    orderedQueryable.ThenByDescending(order[i].MemberExpression);
            }

            return orderedQueryable;
        }

        public static IQueryable<TSource> FilteredBy<TSource>(this IQueryable<TSource> source, Filter<TSource> filter)
        {
            return filter != null ? filter.ApplyFilter(source) : source;
        }

        public static IQueryable<TSource> PagedBy<TSource>(this IQueryable<TSource> source, int page, int pageSize)
        {
            if (page <= 0)
            {
                page = 1;
            }

            return source
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public static IQueryable<TSource> PagedBy<TSource>(this IQueryable<TSource> source, IPagedQueryInput input)
        {
            return source.PagedBy(input.Page, input.PageSize);
        }

        public static IQueryable<TSource> SortedBy<TSource>(this IQueryable<TSource> source, ISortedQueryInput input, string defaultSorting = null)
        {
            var sorting = input.Sorting.CoalesceNullOrWhiteSpace(defaultSorting);
            return !sorting.IsNullOrWhiteSpace() ? source.OrderBy(sorting) : source;
        }

        public static IQueryable<TSource> WhereEqual<TSource, TMember>(this IQueryable<TSource> source, Expression<Func<TSource, TMember>> memberExpression, TMember value)
        {
            var left = memberExpression.Body as MemberExpression;
            var right = Expression.Constant(value, typeof(TMember));
            var param = Expression.Parameter(typeof(TSource), "param");
            var exp = Expression.Equal(left, right);
            return source.Where(Expression.Lambda<Func<TSource, bool>>(exp, param));
        }

        public static IQueryable<TSource> WhereEqual<TSource, TMember>(this IQueryable<TSource> source, string member, TMember value)
        {
            return source.WhereEqual(ExpressionHelper.GetPropertyExpression<TSource, TMember>(member), value);
        }
    }
}
