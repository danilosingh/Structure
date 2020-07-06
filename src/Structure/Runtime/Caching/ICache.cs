using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Structure.Runtime.Caching
{
    public interface ICache : IDisposable
    {
        string Name { get; }

        object Get(string key, Func<string, object> factory);

        Task<object> GetAsync(string key, Func<string, Task<object>> factory);

        object GetOrDefault(string key);

        Task<object> GetOrDefaultAsync(string key);

        void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        void Set(KeyValuePair<string, object>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        Task SetAsync(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        Task SetAsync(KeyValuePair<string, object>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);
        
        bool Contains(string key);

        void Remove(string key);

        void Remove(string[] keys);

        Task RemoveAsync(string key);

        Task RemoveAsync(string[] keys);

        void Clear();

        Task ClearAsync();
    }
}
