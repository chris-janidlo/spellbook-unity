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
		return Random.Range(0, 1) <= probability;
	}
}
}
