using UnityEngine;

namespace crass
{
// Camera.main is not cached, and calls an expensive Transform.Find in order to find the camera [citation needed]. Use this cache when the camera changes rarely.
public static class CameraCache
{
	static Camera main = null;
	public static Camera Main
	{
		get
		{
			if (main != null)
			{
				return main;
			}
			else
			{
				main = Camera.main;
				return main;
			}
		}
	}

	public static void Clear ()
	{
		main = null;
	}
}
}
