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

	// when called every frame, returns true with given probability every second
	public static bool ChancePerSecond (float probability)
	{
		// see https://answers.unity.com/questions/1353041/deltatime-dependent-random-wander-math-problem.html
		// based on the fact that the average probability for an event to happen at least once in n tries is P(A) = 1 - ((1 - P(E))^n), which inverts to P(E) = 1-((1- P(A))^(1/n)). here n is the framerate
		float chance = 1 - Mathf.Pow(1 - probability, Time.deltaTime);
		return Chance(chance);
	}

	public static float Range (Vector2 range)
	{
		return Random.Range(range.x, range.y);
	}

	public static int Range (Vector2Int range)
	{
		return Random.Range(range.x, range.y);
	}
}
}
