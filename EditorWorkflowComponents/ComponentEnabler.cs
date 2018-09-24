using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace crass
{
public class ComponentEnabler : MonoBehaviour
{
	[Tooltip("Components in this list will be _enabled_ on awake in production.")]
	public List<MonoBehaviour> ToEnable;
	[Tooltip("Components in this list will be _disabled_ on awake in production.")]
	public List<MonoBehaviour> ToDisable;

#if !UNITY_EDITOR
	void Awake ()
	{
		foreach (var mb in ToEnable)
		{
			mb.enabled = true;
		}

		foreach (var mb in ToDisable)
		{
			mb.enabled = false;
		}
	}
#endif
}
}
