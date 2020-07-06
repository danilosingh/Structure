using System;
using System.Collections.Generic;
using System.Linq;

namespace Structure.Collections
{
    public class HierarchicalDictionary<TKey, TValue> : Dictionary<TKey, TValue>
        where TValue : IHierarchical<TValue>, IKey<TKey>
    {
        public void Flatten()
        {
            foreach (var key in Keys.ToList())
            {
                AddRecursively(key, this[key]);
            }
        }

        private void AddRecursively(TKey key, TValue value)
        {
            if (TryGetValue(key, out TValue existingValue))
            {
                if (!EqualityComparer<TValue>.Default.Equals(existingValue, value))
                {
                    throw new Exception("Duplicate key detected for " + key);
                }
            }
            else
            {
                this[key] = value;
            }

            foreach (var childValue in value.Children)
            {
                AddRecursively(childValue.Key, childValue);
            }
        }
    }
}
