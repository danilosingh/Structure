using System;
using System.Linq;

namespace Structure.Collections
{
    public static class PagedListExtensions
    {
        public static IPagedList<TCast> As<T, TCast>(this IPagedList<T> pagedList, Func<T, TCast> func)
        {
            return new PagedList<TCast>(pagedList.TotalCount, pagedList.Items.Select(c => func(c)));
        }

        public static IPagedList<TCast> As<TCast>(this IPagedList pagedList)
        {
            return new PagedList<TCast>(pagedList.TotalCount, pagedList.Items.Cast<TCast>());
        }
    }
}
