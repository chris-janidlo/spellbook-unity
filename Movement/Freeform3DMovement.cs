using UnityEngine;

namespace crass
{
// axial movement in 3D space
// you probably want to disable gravity on the rigidbody
[RequireComponent(typeof(Rigidbody))]
public class Freeform3DMovement : MonoBehaviour
{
	public string ForwardKey = "w";
	public string BackwardKey = "s";
	public string LeftKey = "a";
	public string RightKey = "d";
	public string UpKey = "e";
	public string DownKey = "q";

	public Vector3 AxisSpeeds = new Vector3(5,5,5);

	Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}
	
	void Update ()
	{
		Vector3 vel = new Vector3();
		
		if (Input.GetKey(ForwardKey))
			vel.z = AxisSpeeds.z;
		else if (Input.GetKey(BackwardKey))
			vel.z = -AxisSpeeds.z;

		if (Input.GetKey(RightKey))
			vel.x = AxisSpeeds.x;
		else if (Input.GetKey(LeftKey))
			vel.x = -AxisSpeeds.x;

		if (Input.GetKey(UpKey))
			vel.y = AxisSpeeds.y;
		else if (Input.GetKey(DownKey))
			vel.y = -AxisSpeeds.y;

		rb.velocity = transform.TransformDirection(vel);
	}
}
}
