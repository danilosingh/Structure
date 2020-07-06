using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Structure.Collections
{
    public class PagedList<T> : IPagedList<T>
    {
        public int TotalCount { get; }
        public IReadOnlyList<T> Items { get; }

        IList IPagedList.Items
        {
            get
            {
                return Items as IList;
            }
        }

        public PagedList(int totalCount, IEnumerable<T> items)
        {
            TotalCount = totalCount;
            Items = items.ToList();
        }
    }
}
