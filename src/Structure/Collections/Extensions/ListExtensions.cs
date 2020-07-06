using Structure.Collections.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Structure.Collections
{
    public static class ListExtensions
    {
        public static int IndexOf<T>(this IList list, Predicate<T> predicate)
        {
            int i = -1;
            return list.Cast<Object>().Any(x => { i++; return predicate((T)x); }) ? i : -1;
        }

        public static IList<TResult> ForEach<T, TResult>(this IEnumerable<T> list, Func<T, TResult> function)
        {
            List<TResult> results = new List<TResult>();

            foreach (T t in list)
                results.Add(function(t));

            return results;
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }
     
        public static T2 AddReturn<T, T2>(this IList<T> list, T2 item)
            where T2 : T
        {
            list.Add(item);
            return item;
        }

        public static void RemoveAll<T>(this IList<T> list, Predicate<T> predicate)
        {
            int i = list.IndexOf(predicate);

            while (i >= 0)
            {
                list.RemoveAt(i);
                i = list.IndexOf(predicate);
            }
        }

        public static void AddIf<T>(this IList<T> list, bool condition, T item)
        {
            if (condition)
            {
                list.Add(item);
            }
        }
    }
}
