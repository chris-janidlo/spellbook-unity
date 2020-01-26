using System;
using System.Collections;
using UnityEngine;

namespace crass
{
[Serializable]
public abstract class Transitionable<T>
{
	[SerializeField]
	T value;
	public T Value
	{
		get => value;

		// ONLY INTENDED FOR EXTERNAL USE. Set this.value directly inside this class.
		set
		{
			stopCoroutineIfRunning();
			this.value = value;
		}
	}

	[Min(0)]
	public float Time;

	public EasingFunction.Ease Ease = EasingFunction.Ease.Linear;

	MonoBehaviour attachedMonoBehaviour;
	IEnumerator enumerator;
	T currentTarget;

	public void AttachMonoBehaviour (MonoBehaviour monoBehaviour)
	{
		attachedMonoBehaviour = monoBehaviour;
	}

	public void StartTransitionTo (T targetValue, float? timeOverride = null, EasingFunction.Ease? easeOverride = null)
	{
		float time = timeOverride ?? Time;
		EasingFunction.Ease ease = easeOverride ?? Ease;

		if (attachedMonoBehaviour == null)
		{
			throw new InvalidOperationException("cannot start a transition without attaching a MonoBehaviour first");
		}

		if (time == 0)
		{
			value = targetValue;
			return;
		}

		if (time < 0)
		{
			throw new ArgumentException($"transition time cannot be less than 0 seconds (given: {time})");
		}

		if (Value != null && Value.Equals(targetValue)) return;

		currentTarget = targetValue;

		stopCoroutineIfRunning();

		enumerator = transitionRoutine(targetValue, time, ease);
		attachedMonoBehaviour.StartCoroutine(enumerator);
	}

	public void StartTransitionToIfNotAlreadyStarted (T targetValue, float? timeOverride = null, EasingFunction.Ease? easeOverride = null)
	{
		if (enumerator == null || currentTarget == null || !currentTarget.Equals(targetValue))
		{
			StartTransitionTo(targetValue, timeOverride, easeOverride);
		}
	}

	void stopCoroutineIfRunning ()
	{
		if (enumerator != null)
		{
			attachedMonoBehaviour.StopCoroutine(enumerator);
			enumerator = null;
		}
	}

	IEnumerator transitionRoutine (T targetValue, float time, EasingFunction.Ease ease)
	{
		EasingFunction.Function easingFunction  = EasingFunction.GetEasingFunction(ease);

		if (easingFunction == null)
		{
			throw new ArgumentException($"unexpected Ease value {ease}");
		}

		Func<float, float> easeLerpTime = t => easingFunction(0, 1, t);

		T originalValue = Value;
		float timer = 0;

		while (timer < time)
		{
			value = lerp(originalValue, targetValue, easeLerpTime(timer / time));
			timer += UnityEngine.Time.deltaTime;
			yield return null;
		}

		value = targetValue;

		enumerator = null;
	}

	protected abstract T lerp (T a, T b, float t);
}

[Serializable]
public class TransitionableFloat : Transitionable<float>
{
	protected override float lerp (float a, float b, float t) => Mathf.Lerp(a, b, t);
}

[Serializable]
public class TransitionableColor : Transitionable<Color>
{
	protected override Color lerp (Color a, Color b, float t) => Color.Lerp(a, b, t);
}

[Serializable]
public class TransitionableVector2 : Transitionable<Vector2>
{
	protected override Vector2 lerp (Vector2 a, Vector2 b, float t) => Vector2.Lerp(a, b, t);
}

[Serializable]
public class TransitionableVector3 : Transitionable<Vector3>
{
	protected override Vector3 lerp (Vector3 a, Vector3 b, float t) => Vector3.Lerp(a, b, t);
}

[Serializable]
public class TransitionableVector3Slerp : Transitionable<Vector3>
{
	protected override Vector3 lerp (Vector3 a, Vector3 b, float t) => Vector3.Slerp(a, b, t);
}
}