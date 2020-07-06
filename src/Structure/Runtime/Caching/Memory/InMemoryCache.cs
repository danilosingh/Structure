using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;

namespace Structure.Runtime.Caching.Memory
{
    public class InMemoryCache : CacheBase
    {
        private MemoryCache memoryCache;

        public InMemoryCache(string name)
            : base(name)
        {
            memoryCache = new MemoryCache(new OptionsWrapper<MemoryCacheOptions>(new MemoryCacheOptions()));            
        }

        public override object GetOrDefault(string key)
        {
            return memoryCache.Get(key);
        }

        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new Exception("Can not insert null values to the cache!");
            }

            if (absoluteExpireTime != null)
            {
                memoryCache.Set(key, value, DateTimeOffset.Now.Add(absoluteExpireTime.Value));
            }
            else if (slidingExpireTime != null)
            {
                memoryCache.Set(key, value, slidingExpireTime.Value);
            }
            else if (DefaultAbsoluteExpireTime != null)
            {
                memoryCache.Set(key, value, DateTimeOffset.Now.Add(DefaultAbsoluteExpireTime.Value));
            }
            else
            {
                memoryCache.Set(key, value, DefaultSlidingExpireTime);
            }
        }

        public override void Remove(string key)
        {
            memoryCache.Remove(key);
        }

        public override bool Contains(string key)
        {
            return memoryCache.TryGetValue(key, out object _);
        }

        public override void Clear()
        {
            memoryCache.Dispose();
            memoryCache = new MemoryCache(new OptionsWrapper<MemoryCacheOptions>(new MemoryCacheOptions()));
        }

        public override void Dispose()
        {
            memoryCache.Dispose();
            base.Dispose();
        }

    }
}