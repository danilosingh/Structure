using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Runtime.Caching
{
    public static class TypedCacheExtensions
    {
        public static TValue Get<TKey, TValue>(this ITypedCache<TKey, TValue> cache, TKey key, Func<TValue> factory)
        {
            return cache.Get(key, k => factory());
        }

        public static Task<TValue> GetAsync<TKey, TValue>(this ITypedCache<TKey, TValue> cache, TKey key, Func<Task<TValue>> factory)
        {
            return cache.GetAsync(key, k => factory());
        }

        public static Task<TValue> GetCancelableAsync<TKey, TValue>(this ITypedCache<TKey, TValue> cache, TKey key, Func<CancellationToken, Task<TValue>> factory)
        {
            return cache.GetAsync(key, k => factory(new CancellationToken()));
        }
    }
}