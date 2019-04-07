using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace crass
{
// randomly, infinitely cycle through a series of items. shuffles them into bags so that repetitions are infrequent; repetitions can be disabled, too, if the items are unique. use cases include: footstep sounds, tetris pieces, loot tables
[Serializable]
public class BagRandomizer<T>
{
	public List<T> Items;
	[Tooltip("If this is true and every value in Items is unique, GetNext will never return the same value twice.")]
	public bool AvoidRepeats;

	private List<T> bag = new List<T>();
	private int index = 0;

	public T GetNext ()
	{
		if (Items.Count == 0)
		{
			throw new Exception("Items cannot be empty");
		}

		if (index == bag.Count)
		{
			bag = generateNewBag();
			index = 0;
		}

		T tmp = bag[index++];
		return tmp;
	}

	private List<T> generateNewBag ()
	{
		List<T> tmp = new List<T>(Items);
		int tries = 0;

		do
		{
			tmp.ShuffleInPlace();
			tries++;
		}
		while
		(
			// preconditions. if any are false we don't care if we get a repeat
			// check for tries to avoid worse-case performance of O(infinity)
			AvoidRepeats && bag.Count > 0 && Items.Count > 1 && Items.ValuesAreUnique() && tries < 37 &&
			// repetition check
			tmp[0].Equals(bag[bag.Count - 1])
		);

		return tmp;
	}
}
}
