using System.Collections;
using UnityEngine;

namespace crass
{
public static class CameraExtensions
{
	// I don't want to expose screen shake as a coroutine - I want to expose it
	// as an easy method to consume. That means I want the method to call a
	// coroutine. In order to do that, you need an instance of a MonoBehaviour
	// in order to hook into the main thread loop. So we create a GameObject
	// with a dummy MonoBehaviour attached to it.

	class shaker : MonoBehaviour {}
	static GameObject shakingObj;

	// returns true if shake started successfully; false if camera is already shaking
	public static bool ShakeScreen (this Camera camera, float time, float amount)
	{
		if (shakingObj != null) return false;

		shakingObj = new GameObject("shaker", typeof(shaker));
		shakingObj.hideFlags = HideFlags.HideInHierarchy;
		shakingObj.GetComponent<shaker>().StartCoroutine(screenShake(camera, time, amount));

		return true;
	}

	static IEnumerator screenShake (Camera camera, float time, float amount)
	{
		Vector3 originalPosition = camera.transform.localPosition;
		float timer = time;

		while (timer > 0)
		{
			float offset = Mathf.Lerp(amount, 0, 1 - timer / time);
			camera.transform.localPosition = originalPosition + Random.insideUnitSphere * offset;
			timer -= Time.deltaTime;
			yield return null;
		}
		camera.transform.localPosition = originalPosition;

		Object.Destroy(shakingObj);
		shakingObj = null;
	}
}
}