using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    bool isEnabled;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.width -= 28f;
        EditorGUI.BeginDisabledGroup(!isEnabled);
        EditorGUI.PropertyField(position, property, label,true);
        EditorGUI.EndDisabledGroup();
        position.x += position.width + 10f;
        isEnabled = EditorGUI.Toggle(position,isEnabled);
        GUI.enabled = isEnabled;
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
}
