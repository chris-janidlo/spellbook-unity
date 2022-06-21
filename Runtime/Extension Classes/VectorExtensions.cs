using UnityEngine;

namespace crass
{
public static class VectorExtensions
{
	public static Vector2 Decelerate (this Vector2 currentSpeed, float decelerationPerSecond)
	{
		Vector2 velocityChange = -currentSpeed.normalized * decelerationPerSecond;
		velocityChange = Vector2.ClampMagnitude(velocityChange, currentSpeed.magnitude);
		return currentSpeed + velocityChange * Time.deltaTime;
	}

	public static Vector3 Decelerate (this Vector3 currentSpeed, float decelerationPerSecond)
	{
		Vector3 velocityChange = -currentSpeed.normalized * decelerationPerSecond;
		velocityChange = Vector3.ClampMagnitude(velocityChange, currentSpeed.magnitude);
		return currentSpeed + velocityChange * Time.deltaTime;
	}

	public static (Vector2, float) DirectionAndDistance (Vector2 a, Vector2 b)
	{
		Vector2 heading = b - a;
		float distance = heading.magnitude;
		Vector2 direction = heading / distance;
		return (direction, distance);
	} 

	public static (Vector3, float) DirectionAndDistance (Vector3 a, Vector3 b)
	{
		Vector3 heading = b - a;
		float distance = heading.magnitude;
		Vector3 direction = heading / distance;
		return (direction, distance);
	} 
}
}
