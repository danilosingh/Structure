using Structure.Collections.Extensions;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Structure.Runtime.Caching
{
    public abstract class CacheManagerBase : ICacheManager
    {
        protected readonly ConcurrentDictionary<string, ICache> Caches;

        protected CacheManagerBase()
        {
            Caches = new ConcurrentDictionary<string, ICache>();
        }

        public IReadOnlyList<ICache> GetAllCaches()
        {
            return Caches.Values.AsReadOnly();
        }

        public virtual ICache GetCache(string name)
        {
            Check.NotNull(name, nameof(name));

            return Caches.GetOrAdd(name, (cacheName) =>
            {
                var cache = CreateCacheImplementation(cacheName);
                return cache;
            });
        }
        
        protected abstract void DisposeCaches();

        public virtual void Dispose()
        {
            DisposeCaches();
            Caches.Clear();
        }

        protected abstract ICache CreateCacheImplementation(string name);
    }
}
