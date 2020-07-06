using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Structure.Collections
{
    public class PagedQueryable<TSource> : IQueryable<TSource>
    {
        private readonly IQueryable<TSource> source;

        public int Page { get; private set; }
        public int PageSize { get; private set; }

        public Expression Expression
        {
            get { return source.Expression; }
        }

        public Type ElementType
        {
            get { return source.ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return source.Provider; }
        }

        public PagedQueryable(IQueryable<TSource> source, int page, int pageSize)
        {
            if (page <= 0)
                page = 1;

            this.source = source
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            Page = page;
            PageSize = pageSize;
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            return source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return source.GetEnumerator();
        }
    }
}
