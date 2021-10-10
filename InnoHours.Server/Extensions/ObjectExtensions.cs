using System;
using System.Collections.Generic;

namespace InnoHours.Server.Extensions
{
    public static class ObjectExtensions
    {
        public static TOut Let<TOut, TIn>(this TIn obj, Func<TIn, TOut> function)
        {
            return function(obj);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}