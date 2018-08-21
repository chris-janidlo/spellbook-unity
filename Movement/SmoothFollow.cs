using UnityEngine;

namespace crass
{
public class SmoothFollow : MonoBehaviour
{
	public Transform Target;
	public float MoveDelay, RotationFactor;

	Vector3 velocity = Vector3.zero;

	void Start ()
	{
		if (transform.parent != null)
		{
			transform.parent = null;
		}
	}
	
	void Update ()
	{
		transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref velocity, MoveDelay);
		transform.rotation = Quaternion.Slerp(transform.rotation, Target.rotation, RotationFactor);
	}
}
}
