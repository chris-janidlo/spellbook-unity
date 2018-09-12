using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace crass
{
public class CameraRayCaster : Singleton<CameraRayCaster>
{
	public LayerMask CollisionMask;
	public float Distance;

	public IClickableObject HoveredObject { get; private set; }

	void Start ()
	{
		SingletonSetInstance(this, false);
	}
	
	void Update ()
	{
		raycast();

		if (HoveredObject != null && Input.GetMouseButtonDown(0))
		{
			HoveredObject.Click();
		}
	}

	void raycast ()
	{
		RaycastHit hitInfo;
		
		if (Physics.Raycast(transform.position, transform.forward, out hitInfo, Distance, CollisionMask, QueryTriggerInteraction.Ignore))
		{
			var clickable = hitInfo.collider.gameObject.GetComponent<IClickableObject>();

			if (clickable == null)
			{
				HoveredObject = null;
			}
			else if (clickable != HoveredObject)
			{
				HoveredObject = clickable;
			}
		}
		else
		{
			HoveredObject = null;
		}

	}

}
}
