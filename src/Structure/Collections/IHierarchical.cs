using System.Collections.Generic;

namespace Structure.Collections
{
    public interface IHierarchical<T>
    {
        IEnumerable<T> Children { get; }
    }
}
