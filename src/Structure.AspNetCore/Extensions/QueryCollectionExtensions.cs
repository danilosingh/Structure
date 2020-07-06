using Structure.Helpers;
using Microsoft.AspNetCore.Http;

namespace Structure.AspNetCore.Extensions
{
    public static class QueryCollectionExtensions
    {
        public static bool TryGet<T>(this IQueryCollection collection, string key, out T value)
        {
            value = default;

            if (!collection.TryGetValue(key, out var internalValue))
            {
                return false;
            }

            try
            {
                value = TypeHelper.Convert<T>(internalValue[0]);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
