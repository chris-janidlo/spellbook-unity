using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScriptableInput
{
	static ScriptableInput ()
	{
		Mapping = new Dictionary<string, InputAxis>
		{
			{ "Jet", new InputAxis
			{
				Positive = KeyCode.W,
				Negative = KeyCode.S,
			}},
			{ "Lift", new InputAxis
			{
				Positive = KeyCode.Space,
				Negative = KeyCode.LeftShift,
				AltPositive = KeyCode.E,
				AltNegative = KeyCode.Q,
			}},
			{ "Strafe", new InputAxis
			{
				Positive = KeyCode.D,
				Negative = KeyCode.A,
			}},
			{ "Pitch", new InputAxis
			{
				Positive = KeyCode.DownArrow,
				Negative = KeyCode.UpArrow,
			}},
			{ "Turn", new InputAxis
			{
				Positive = KeyCode.RightArrow,
				Negative = KeyCode.LeftArrow,
			}},
		};
		foreach (KeyValuePair<string, InputAxis> entry in Mapping)
		{
			entry.Value.Sensitivity = 3f;
			entry.Value.Gravity = 3f;
		}
	}

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
