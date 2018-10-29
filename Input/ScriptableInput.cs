using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScriptableInput
{
	public static Dictionary<string, InputAxis> Mapping;

	public static float GetAxis (string name)
	{
		return Mapping[name].GetInput();;
	}
}

public class InputAxis
{
	public KeyCode Positive, Negative, AltPositive, AltNegative;
	public float Gravity, Sensitivity;

	float target, value;

	public float GetInput ()
	{
		// TODO: fix smoothing to better emulate Unity's build-int input smoothing
		
		bool
			p = Input.GetKey(Positive) || Input.GetKey(AltPositive),
			n = Input.GetKey(Negative) || Input.GetKey(AltNegative);
		
		if (!p && !n)
		{
			target = 0f;
			value = Mathf.MoveTowards(value, target, Gravity * Time.deltaTime);
			return value;
		}
		else if (target != 0f)
		{
			return target;
		}
		else
		{
			// prefer positive
			target = p ? 1f : -1f;
			value = Mathf.MoveTowards(value, target, Sensitivity * Time.deltaTime);
			return value;
		}
	}
}
