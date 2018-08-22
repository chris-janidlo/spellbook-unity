using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace crass
{
// TODO: change implementation to linq
[Serializable]
public class BagRandomizer<T>
{
	public List<T> Items;
	[Tooltip("Set this to true to prevent items from appearing twice in a row. Only works if every item in items is unique.")]
	public bool ItemsAreUnique;

	private List<T> bag = new List<T>();
	private int index = 0;

	public T GetNext ()
	{
		if (bag.Count == 0 || index == bag.Count)
		{
			bag = generateNewBag();
			index = 0;
		}
		T tmp = bag[index++];
		return tmp;
	}

	private List<T> generateNewBag ()
	{
		List<T> tmp;
		int tries = 0;

		do
		{
			tmp = shuffledClone(Items);
			tries++;
		}
		while
		(
			ItemsAreUnique && bag.Count > 0 && tries < 37 && tmp[0].Equals(bag[bag.Count - 1])
		);

		return tmp;
	}

	private List<T> shuffledClone (IList<T> list)
	{
		List<T> output = new List<T>(list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			int j = UnityEngine.Random.Range(0, i+1);
			if (j == output.Count)
			{
				output.Add(list[i]);
			}
			else
			{
				output.Add(output[j]);
				output[j] = list[i];
			}
		}
		return output;
	}
}
}
