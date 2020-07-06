using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Runtime.Caching
{
    public abstract class CacheBase : ICache
    {
        protected readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);
        protected readonly object SyncObj = new object();

        public string Name { get; }

        public TimeSpan DefaultSlidingExpireTime { get; set; }

        public TimeSpan? DefaultAbsoluteExpireTime { get; set; }

        protected CacheBase(string name)
        {
            Name = name;
            DefaultSlidingExpireTime = TimeSpan.FromHours(1);
        }

        public virtual object Get(string key, Func<string, object> factory)
        {
            object item = GetOrDefault(key);

            if (item != null)
            {
                return item;
            }

            return GetOrDefault(key, factory);
        }

        private object GetOrDefault(string key, Func<string, object> factory)
        {
            lock (SyncObj)
            {
                var item = GetOrDefault(key);

                if (item != null)
                {
                    return item;
                }

                item = factory(key);

                if (item == null)
                {
                    return null;
                }

                Set(key, item);
                return item;
            }
        }

        public virtual async Task<object> GetAsync(string key, Func<string, Task<object>> factory)
        {
            object item = await GetOrDefaultAsync(key);

            if (item != null)
            {
                return item;
            }

            return await GetOrDefaultAsync(key, factory);
        }

        private async Task<object> GetOrDefaultAsync(string key, Func<string, Task<object>> factory)
        {
            try
            {
                await Semaphore.WaitAsync();

                var item = await GetOrDefaultAsync(key);

                if (item != null)
                {
                    return item;
                }

                item = await factory(key);

                if (item == null)
                {
                    return null;
                }

                await SetAsync(key, item);
                return item;
            }
            finally
            {
                Semaphore.Release();
            }
        }

        public abstract object GetOrDefault(string key);

        public virtual Task<object> GetOrDefaultAsync(string key)
        {
            return Task.FromResult(GetOrDefault(key));
        }

        public abstract void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        public virtual void Set(KeyValuePair<string, object>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            foreach (var pair in pairs)
            {
                Set(pair.Key, pair.Value, slidingExpireTime, absoluteExpireTime);
            }
        }

        public virtual Task SetAsync(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            Set(key, value, slidingExpireTime, absoluteExpireTime);
            return Task.FromResult(0);
        }

        public virtual Task SetAsync(KeyValuePair<string, object>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            return Task.WhenAll(pairs.Select(p => SetAsync(p.Key, p.Value, slidingExpireTime, absoluteExpireTime)));
        }

        public abstract void Remove(string key);

        public virtual void Remove(string[] keys)
        {
            foreach (var key in keys)
            {
                Remove(key);
            }
        }

        public virtual Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.FromResult(0);
        }

        public virtual Task RemoveAsync(string[] keys)
        {
            return Task.WhenAll(keys.Select(RemoveAsync));
        }

        public abstract void Clear();

        public virtual Task ClearAsync()
        {
            Clear();
            return Task.FromResult(0);
        }

        public virtual void Dispose()
        {

        }

        public abstract bool Contains(string key);
    }
}
