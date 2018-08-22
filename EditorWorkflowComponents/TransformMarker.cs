#if UNITY_EDITOR

using UnityEngine;

namespace crass
{
public class TransformMarker : MonoBehaviour
{
	public float Radius = .1f;
	
	void OnDrawGizmos ()
	{
		Gizmos.DrawWireSphere(transform.position, Radius);
	}
}
}

#endif
