using UnityEditor;

namespace crass
{
[CustomEditor(typeof(AbstractPrefab))]
public class AbstractPrefabEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This prefab is abstract. Do not instantiate it.", MessageType.Warning);
    }
}
}
