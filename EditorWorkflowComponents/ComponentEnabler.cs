using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace crass
{
public class ComponentEnabler : MonoBehaviour
{
	[Tooltip("Components in this list will be _enabled_ on awake in production.")]
	public List<Component> ToEnable;
	[Tooltip("Components in this list will be _disabled_ on awake in production.")]
	public List<Component> ToDisable;

#if !UNITY_EDITOR
	void Awake ()
	{
		foreach (var com in ToEnable)
		{
			com.enabled = true;
		}

		foreach (var com in ToDisable)
		{
			com.enabled = false;
		}
	}
#endif
}
}
