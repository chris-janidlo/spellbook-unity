// credit to FatiguedArtist on the Unity forums
// https://forum.unity.com/threads/a-free-simple-smooth-mouselook.73117/

using UnityEngine;

namespace crass
{
public class MouseLook : MonoBehaviour
{
	public Vector2 ClampInDegrees = new Vector2(360, 180);
	public bool LockCursor;
	public Vector2 Sensitivity = new Vector2(2, 2);

	[Tooltip("Higher values mean less jittery input when moving the mouse, but adds lag and makes input less precise. Set to (1, 1) to remove all smoothing.")]
	public Vector2 Smoothing = new Vector2(1, 1);

	[Tooltip("Assign this if there's a parent object controlling motion, such as a Character Controller. Yaw rotation will affect this object instead of the camera if set.")]
	public GameObject CharacterBody;

	Vector2 mousePosition;
	Vector2 smoothedMouseInput;
	Vector2 targetDirection;
	Vector2 targetCharacterDirection;

	CursorLockMode previousLockMode;

	void Start ()
	{
		// Set target direction to the camera's initial orientation.
		targetDirection = transform.localRotation.eulerAngles;

		// Set target direction for the character body to its inital state.
		if (CharacterBody)
			targetCharacterDirection = CharacterBody.transform.localRotation.eulerAngles;

		previousLockMode = Cursor.lockState;
	}

	void Update ()
	{
		// Ensure the cursor is always locked when set
		if (LockCursor)
		{
			Cursor.lockState = CursorLockMode.Locked;
		}

		// Allow the script to clamp based on a desired target value.
		var targetOrientation = Quaternion.Euler(targetDirection);
		var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

		// Get raw mouse input for a cleaner reading on more sensitive mice.
		var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

		// Scale input against the sensitivity setting and multiply that against the smoothing value.
		mouseDelta = Vector2.Scale(mouseDelta, new Vector2(Sensitivity.x * Smoothing.x, Sensitivity.y * Smoothing.y));

		// Interpolate mouse movement over time to apply smoothing delta.
		smoothedMouseInput.x = Mathf.Lerp(smoothedMouseInput.x, mouseDelta.x, 1f / Smoothing.x);
		smoothedMouseInput.y = Mathf.Lerp(smoothedMouseInput.y, mouseDelta.y, 1f / Smoothing.y);

		// Find the absolute mouse movement value from point zero.
		mousePosition += smoothedMouseInput;

		// Clamp and apply the local x value first, so as not to be affected by world transforms.
		if (ClampInDegrees.x < 360)
			mousePosition.x = Mathf.Clamp(mousePosition.x, -ClampInDegrees.x * 0.5f, ClampInDegrees.x * 0.5f);

		// Then clamp and apply the global y value.
		if (ClampInDegrees.y < 360)
			mousePosition.y = Mathf.Clamp(mousePosition.y, -ClampInDegrees.y * 0.5f, ClampInDegrees.y * 0.5f);

		transform.localRotation = Quaternion.AngleAxis(-mousePosition.y, targetOrientation * Vector3.right) * targetOrientation;

		// If there's a character body that acts as a parent to the camera
		if (CharacterBody)
		{
			var yRotation = Quaternion.AngleAxis(mousePosition.x, Vector3.up);
			CharacterBody.transform.localRotation = yRotation * targetCharacterOrientation;
		}
		else
		{
			var yRotation = Quaternion.AngleAxis(mousePosition.x, transform.InverseTransformDirection(Vector3.up));
			transform.localRotation *= yRotation;
		}
	}

	void OnDestroy ()
	{
		Cursor.lockState = previousLockMode;
	}
}
}
