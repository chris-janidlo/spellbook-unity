using UnityEngine;

namespace crass
{
public class SmoothFollow : MonoBehaviour
{
	public Transform Target;
	public Vector3 OffsetFromTarget;

	[Tooltip("How many seconds it takes for this object to reach the target's position plus the offset")]
	public float MoveDelay = 0.3f;
	
	[Tooltip("How many seconds it takes for this object's rotation to reach the target's rotation")]
	public float RotationDelay = 0.3f;

	Vector3 positionvelocity = Vector3.zero;
	Quaternion rotationVelocity = Quaternion.identity;

	void Start ()
	{
		if (transform.parent != null)
		{
			transform.parent = null;
		}
	}
	
	void Update ()
	{
		transform.position = Vector3.SmoothDamp(transform.position, Target.TransformPoint(OffsetFromTarget), ref positionvelocity, MoveDelay);
		transform.rotation = transform.rotation.SmoothDampTo(Target.rotation, ref rotationVelocity, RotationDelay);
	}
}
}
