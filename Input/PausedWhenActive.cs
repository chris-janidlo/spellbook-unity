using UnityEngine;

// pauses the game while the GameObject is active
public class PausedWhenActive : MonoBehaviour
{
	[Tooltip("If this is true, pause when *inactive* instead of when active.")]
	public bool Flip;

	float previousTimeScale;

	void OnEnable ()
	{
		setPauseState(!Flip);
	}

	void OnDisable ()
	{
		setPauseState(Flip);		
	}

	void setPauseState (bool value)
	{
		if (value)
		{
			previousTimeScale = Time.timeScale;
			Time.timeScale = 0;
		}
		else
		{
			Time.timeScale = previousTimeScale;
		}
	}
}
