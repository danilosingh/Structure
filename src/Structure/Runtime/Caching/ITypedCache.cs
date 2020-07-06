using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Structure.Runtime.Caching
{
    public interface ITypedCache<TKey, TValue> : IDisposable
    {
        string Name { get; }

        ICache InternalCache { get; }

        TValue Get(TKey key, Func<TKey, TValue> factory);

        Task<TValue> GetAsync(TKey key, Func<TKey, Task<TValue>> factory);

        TValue GetOrDefault(TKey key);

        Task<TValue> GetOrDefaultAsync(TKey key);

        void Set(TKey key, TValue value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);
        
        void Set(KeyValuePair<TKey, TValue>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        Task SetAsync(TKey key, TValue value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        Task SetAsync(KeyValuePair<TKey, TValue>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        void Remove(TKey key);

        Task RemoveAsync(TKey key);

        void Clear();

        Task ClearAsync();
        bool Contains(TKey key);
    }
}
