using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace crass
{
public static class RandomExtra
{
	// returns true with given probability
	public static bool Chance (float probability)
	{
		if (probability < 0 || probability > 1)
		{
			throw new System.Exception($"Given probability {probability} is outside of range [0, 1]");
		}
		return Random.Range(0f, 1f) <= probability;
	}
}
}
