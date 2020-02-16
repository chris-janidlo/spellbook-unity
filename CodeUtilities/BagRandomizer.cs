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
	public enum PRNGType
	{
		Global,
		Local,
		LocalSeeded
	}

	public List<T> Items;

	[Tooltip("If this is true and every value in Items is unique, GetNext will never return the same value twice.")]
	public bool AvoidRepeats;

	[Tooltip("Sets which PRNG to use. Global uses the global UnityEngine.Random, while the Local values instantiate one PRNG per BagRandomizer. If set to LocalSeeded, the LocalSeed value is used; if set to Local, an arbitrary seed is chosen.")]
	public PRNGType Type;

	[Tooltip("The seed used by the local PRNG when Type is set to PRNGType.LocalSeeded.")]
	public int LocalSeed;

	System.Random _localRandom;
	System.Random localRandom
	{
		get
		{
			if (_localRandom == null)
				_localRandom = new System.Random(Type == PRNGType.LocalSeeded ? LocalSeed : Environment.TickCount);

			return _localRandom;
		}
	}

	bool actuallyAvoidRepeats => AvoidRepeats && Items.ValuesAreUnique() && Items.Count >= 2;

	List<T> bag;
	int _index = -1;

	T previousItem;
	bool avoidingPrevious;

	public T PeekNext ()
	{
		if (Items.Count == 0)
			throw new InvalidOperationException("Items cannot be empty");

		if (bag == null || bag.Count == 0)
		{
			bag = new List<T>(Items);

			if (actuallyAvoidRepeats && avoidingPrevious)
			{
				bag.Remove(previousItem);
			}
		}

		return bag[currentIndex()];
	}

	public T GetNext ()
	{
		T next = PeekNext();

		bag.RemoveAt(currentIndex());
		clearCurrentIndex();

		if (avoidingPrevious)
		{
			avoidingPrevious = false;
			bag.Add(previousItem);
		}

		if (actuallyAvoidRepeats && bag.Count == 0)
		{
			avoidingPrevious = true;
			previousItem = next;
		}

		return next;
	}

	public void SetPRNG (System.Random prng)
	{
		_localRandom = prng;
	}

	int currentIndex ()
	{
		if (_index >= 0) return _index;

		switch (Type)
		{
			case PRNGType.Global:
				return _index = UnityEngine.Random.Range(0, bag.Count);
			
			case PRNGType.Local:
			case PRNGType.LocalSeeded:
				return _index = localRandom.Next(bag.Count);

			default:
				throw new InvalidOperationException($"unexpected PRNGType {Type}");
		}
	}

	void clearCurrentIndex ()
	{
		_index = -1;
	}
}
}
