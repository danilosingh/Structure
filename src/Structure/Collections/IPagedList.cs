using System.Collections;
using System.Collections.Generic;

namespace Structure.Collections
{
    public interface IPagedList 
    {
        int TotalCount { get; }
        IList Items { get; }
    }

    public interface IPagedList<out T> : IPagedList
    {
        new int TotalCount { get; } //add devido a não serialização no retorno da api (refatorar)
        new IReadOnlyList<T> Items { get; }
    }
}
