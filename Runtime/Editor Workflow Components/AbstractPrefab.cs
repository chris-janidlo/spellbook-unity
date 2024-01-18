using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace crass
{
    public class AbstractPrefab : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        string prefabName;

#if UNITY_EDITOR
        void OnValidate()
        {
            if (!Application.isPlaying)
            {
                prefabName = PrefabUtility.GetCorrespondingObjectFromSource(gameObject)?.name;
            }
        }
#endif

        void Update()
        {
            string errorMessage = !string.IsNullOrWhiteSpace(prefabName)
                ? $"Prefab '{prefabName}' is marked as abstract. Don't instantiate it directly; instead, instantiate a variant of it without the {nameof(AbstractPrefab)} component. (from object '{name}')"
                : $"An {nameof(AbstractPrefab)} component is attached to object '{name}'. Either it was instantiated from an abstract prefab, or the component was attached when it should not have been.";

            Debug.LogError(errorMessage);
        }
    }
}
