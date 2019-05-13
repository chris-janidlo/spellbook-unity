using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace crass
{
public static class ListExtensions
{
	public static void ShuffleInPlace<T> (this IList<T> list)
	{
		int n = list.Count;
		while (n --> 1)
		{
			int k = Random.Range(0, n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	public static bool ValuesAreUnique<T> (this IList<T> list)
	{
		return list.Distinct().Count() == list.Count;
	}

	public static T PickRandom<T> (this IList<T> list)
	{
		return list[Random.Range(0, list.Count)];
	}
}
}
