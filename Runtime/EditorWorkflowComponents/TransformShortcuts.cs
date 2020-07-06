#if UNITY_EDITOR

using UnityEngine;

namespace crass
{
// put this on a complicated transform hierarchy's root, then assign points of
// interest. in the editor, you can click on these to then quickly go to the
// appropriate transform.
public class TransformShortcuts : MonoBehaviour
{
	public Transform[] TransformsOfInterest;
}
}

#endif
