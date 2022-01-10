using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace crass
{
    [CustomPropertyDrawer(typeof(EnumMap<,>))]
    public class EnumMapDrawer : PropertyDrawer
    {
        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect foldoutPosition = position;
            foldoutPosition.height = foldoutLabelHeight(label);
            property.isExpanded = EditorGUI.Foldout(foldoutPosition, property.isExpanded, label, true);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                Rect fieldPosition = EditorGUI.IndentedRect(position);
                fieldPosition.y += foldoutLabelHeight(label) + EditorGUIUtility.standardVerticalSpacing; // ensure fields start below the label

                var enums = property.FindPropertyRelative("m_enums");
                var values = property.FindPropertyRelative("m_values");

                for (int i = 0; i < enums.arraySize; i++)
                {
                    var value = values.GetArrayElementAtIndex(i);

                    var currentEnum = enums.GetArrayElementAtIndex(i);
                    string enumLabel = currentEnum.enumDisplayNames[currentEnum.enumValueIndex];

                    fieldPosition.height = EditorGUI.GetPropertyHeight(value);
                    EditorGUI.PropertyField(fieldPosition, value, new GUIContent(enumLabel), true);
                    fieldPosition.y += fieldPosition.height + EditorGUIUtility.standardVerticalSpacing;
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded) return base.GetPropertyHeight(property, label);

            var values = property.FindPropertyRelative("m_values");

            float height = foldoutLabelHeight(label) + EditorGUIUtility.standardVerticalSpacing; // include the label and some space
            for (int i = 0; i < values.arraySize; i++)
            {
                height += EditorGUI.GetPropertyHeight(values.GetArrayElementAtIndex(i), true) + EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }

        float foldoutLabelHeight (GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(SerializedPropertyType.String, label);
        }
    }
}
