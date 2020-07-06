using System;
using System.Collections.Generic;

namespace Structure.Runtime.Caching
{
    public interface ICacheManager : IDisposable
    {
        IReadOnlyList<ICache> GetAllCaches();
        ICache GetCache(string name);
    }
}
