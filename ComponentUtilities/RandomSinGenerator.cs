using UnityEngine;

namespace crass
{
public class RandomSinGenerator : MonoBehaviour
{
	public Vector2 FrequencyRange;
	public float Amplitude, MaxStartingPhase;

	public float Phase { get; private set; }

	void Start ()
	{
		Phase = Random.value * MaxStartingPhase;
	}

	void Update ()
	{
		float frequency = Random.Range(FrequencyRange.x, FrequencyRange.y);
		Phase += (Time.deltaTime * frequency);
	}
}
}
