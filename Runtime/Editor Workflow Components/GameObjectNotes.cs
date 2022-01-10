using UnityEngine;

// TODO: make this an editor script instead of a monobehaviour

namespace crass
{
public class GameObjectNotes : MonoBehaviour
{
	[TextArea(5, 5000)]
	public string Notes;
}
}
