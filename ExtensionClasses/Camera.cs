using System.Collections;
using UnityEngine;

namespace crass
{
public static class CameraExtensions
{
	static bool shaking;

	public static bool ShakeScreen (this Camera camera, float time, float amount)
	{
		if (shaking) return false;
		shaking = true;

		(new MonoBehaviour()).StartCoroutine(screenShake(camera, time, amount));

		return true;
	}

	static IEnumerator screenShake (Camera camera, float time, float amount)
	{
		Vector3 originalPosition = camera.transform.position;
		float timer = time;
		while (timer > 0)
		{
			float offset = Mathf.Lerp(amount, 0, timer / time);
			camera.transform.position = originalPosition + Random.insideUnitSphere * offset;
			timer -= Time.deltaTime;
			yield return null;
		}
		camera.transform.position = originalPosition;
		shaking = false;
	}
}
}