namespace Structure.Runtime.Caching.Memory
{
    public class InMemoryCacheManager : CacheManagerBase
    {
        public InMemoryCacheManager()
        { }

        protected override ICache CreateCacheImplementation(string name)
        {
            return new InMemoryCache(name);
        }

        protected override void DisposeCaches()
        {
            foreach (var cache in Caches.Values)
            {
                cache.Dispose();
            }
        }
    }
}
