using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace crass
{
    [CustomPropertyDrawer(typeof(EnumMap<,>))]
    public class EnumMapDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            try
            {
                DrawEnumMap(position, property, label);
            }
            catch (Exception)
            {
                EditorGUILayout.HelpBox(
                    "There was an error when drawing this. Make sure that the value "
                        + "type for this EnumMap can be serialized as part of a List; "
                        + "for example, Unity cannot serialize nested Lists.",
                    MessageType.Error
                );
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            try
            {
                return EnumMapPropertyHeight(property, label);
            }
            catch (Exception)
            {
                return base.GetPropertyHeight(property, label);
            }
        }

        private void DrawEnumMap(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect foldoutPosition = position;
            foldoutPosition.height = FoldoutLabelHeight(label);
            property.isExpanded = EditorGUI.Foldout(
                foldoutPosition,
                property.isExpanded,
                label,
                true
            );

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                Rect fieldPosition = EditorGUI.IndentedRect(position);
                fieldPosition.y +=
                    FoldoutLabelHeight(label) + EditorGUIUtility.standardVerticalSpacing; // ensure fields start below the label

                var enums = property.FindPropertyRelative("m_enums");
                var values = property.FindPropertyRelative("m_values");

                for (int i = 0; i < enums.arraySize; i++)
                {
                    var value = values.GetArrayElementAtIndex(i);

                    var currentEnum = enums.GetArrayElementAtIndex(i);
                    string enumLabel = currentEnum.enumDisplayNames[currentEnum.enumValueIndex];

                    fieldPosition.height = EditorGUI.GetPropertyHeight(value);
                    EditorGUI.PropertyField(fieldPosition, value, new GUIContent(enumLabel), true);
                    fieldPosition.y +=
                        fieldPosition.height + EditorGUIUtility.standardVerticalSpacing;
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        private float EnumMapPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return base.GetPropertyHeight(property, label);

            var values = property.FindPropertyRelative("m_values");

            float height = FoldoutLabelHeight(label) + EditorGUIUtility.standardVerticalSpacing; // include the label and some space
            for (int i = 0; i < values.arraySize; i++)
            {
                height +=
                    EditorGUI.GetPropertyHeight(values.GetArrayElementAtIndex(i), true)
                    + EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }

        private float FoldoutLabelHeight(GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(SerializedPropertyType.String, label);
        }
    }
}
