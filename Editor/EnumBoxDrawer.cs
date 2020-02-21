using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace crass
{
// TODO: as soon as Unity starts using UIElements as the basis for the default inspector, switch to UIElements
public class EnumBoxDrawer<TEnum, TValue> : PropertyDrawer
	where TEnum : Enum
	where TValue : struct
{
	static IEnumerable<TEnum> _cachedEnumValues;
	static IEnumerable<TEnum> cachedEnumValues => _cachedEnumValues ?? (_cachedEnumValues = EnumUtil.AllValues<TEnum>());

    static int _cachedEnumCount = -1;
    static int cachedEnumCount => _cachedEnumCount == -1 ? (_cachedEnumCount = EnumUtil.NameCount<TEnum>()) : _cachedEnumCount;

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		SerializedProperty arrayProp = property.FindPropertyRelative("_values");

		float totalHeight = 0;

		for (int i = 0; i < cachedEnumCount; i++)
		{
			// need to calculate height based off of elements instead of the total array since the total array height includes the "size" field that we don't want
			totalHeight += EditorGUI.GetPropertyHeight(arrayProp.GetArrayElementAtIndex(i), label, false) + EditorGUIUtility.standardVerticalSpacing;
		}
 
		return totalHeight;
	}

	// TODO: could be cleaner
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		position = EditorGUI.PrefixLabel(position, label);

		SerializedProperty arrayProp = property.FindPropertyRelative("_values");

		int i = 0;
		float prevHeight = 0;

		foreach (TEnum evalue in cachedEnumValues)
		{
			SerializedProperty currentValue = arrayProp.GetArrayElementAtIndex(i++);
			GUIContent nextLabel = new GUIContent(evalue.ToString());
			Rect newRect = new Rect(position.x, position.y + prevHeight + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUI.GetPropertyHeight(currentValue, nextLabel, true));

			prevHeight += newRect.height + EditorGUIUtility.standardVerticalSpacing;
			EditorGUI.PropertyField(newRect, currentValue, nextLabel, true);
		}

		EditorGUI.EndProperty();
	}
}
}
