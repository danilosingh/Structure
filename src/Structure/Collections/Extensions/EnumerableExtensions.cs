using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Structure.Collections.Extensions
{
    public static class EnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> list, Predicate<T> predicate)
        {
            int i = -1;

            if (list == null)
            {
                return i;
            }

            return list.Any(x =>
            {
                i++;
                return predicate(x);
            }) ? i : -1;
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (T t in list)
                action(t);
        }

        public static string Join(this IEnumerable<string> list, string separator = null)
        {
            return string.Join(separator ?? string.Empty, list);
        }

        public static ReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable is List<T> list)
            {
                return list.AsReadOnly();
            }

            return enumerable.ToList().AsReadOnly();
        }

        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int totalCount)
        {
            return new PagedList<T>(totalCount, source);
        }

        public static IPagedList<TResult> ToPagedList<T, TResult>(this IEnumerable<T> source, Func<T, TResult> adapter, int totalCount)
        {
            return source.Select(adapter).ToList().ToPagedList(totalCount);
        }
    }
}
