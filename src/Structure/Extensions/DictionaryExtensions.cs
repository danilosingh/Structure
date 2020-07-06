using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Structure.Extensions
{
    public static class DictionaryExtensions
    {
        public static void CreateOrUpdate<TKey, TValue>(this IDictionary dictionary, TKey key, TValue value)
        {
            if (dictionary.Contains(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }            
        }

        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : default(TValue);
        }

        public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey,TValue>, bool> predicate)
        {
            foreach (var item in dictionary.Where(predicate))
            {
                dictionary.Remove(item.Key);
            }
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory)
        {
            TValue obj;
            if (dictionary.TryGetValue(key, out obj))
            {
                return obj;
            }

            return dictionary[key] = factory(key);
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> factory)
        {
            return dictionary.GetOrAdd(key, k => factory());
        }
    }
}
