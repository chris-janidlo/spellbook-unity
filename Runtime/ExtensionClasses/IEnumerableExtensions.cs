using System;
using System.Collections.Generic;

namespace crass
{
public static class IEnumberableExtensions
{
	public static IEnumerable<TSource> DistinctBy<TSource, TKey> (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = new HashSet<TKey>();

        foreach (TSource element in source)
        {
			TKey key = keySelector(element);

            if (!seenKeys.Contains(key))
            {
				seenKeys.Add(key);
                yield return element;
            }
        }
    }
}
}
