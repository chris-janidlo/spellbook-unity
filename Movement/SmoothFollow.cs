using UnityEngine;

namespace crass
{
public class SmoothFollow : MonoBehaviour
{
	public Transform Target;
	public Vector3 OffsetFromTarget;

	[Tooltip("How many seconds it takes for this object to reach the offset target position")]
	public float MoveDelay;
	
	[Range(0, 1)]
	[Tooltip("How relatively fast the object matches the target rotation. 0 doesn't match at all, 1 matches instantaneously")]
	public float RotationFactor;

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
		transform.position = Vector3.SmoothDamp(transform.position, Target.position + OffsetFromTarget, ref velocity, MoveDelay);
		transform.rotation = Quaternion.Slerp(transform.rotation, Target.rotation, RotationFactor);
	}
}
}
