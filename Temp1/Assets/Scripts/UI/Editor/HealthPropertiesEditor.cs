using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is only used to set inspector properties correctly
/// </summary>
[CustomEditor(typeof(HealthPreferences))]
public class HealthPropertiesEditor : Editor
{
    override public void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        var preferences = target as HealthPreferences;

        switch (preferences.fillMethod)
        {
            case Image.FillMethod.Horizontal:
                SerializedProperty horizontalDirection = serializedObject.FindProperty("horizontalDirection");
                EditorGUILayout.PropertyField(horizontalDirection);
                break;
            case Image.FillMethod.Vertical:
                SerializedProperty verticalDirection = serializedObject.FindProperty("verticalDirection");
                EditorGUILayout.PropertyField(verticalDirection);
                break;
            case Image.FillMethod.Radial360:
                SerializedProperty radial360Direction = serializedObject.FindProperty("radial360Direction");
                EditorGUILayout.PropertyField(radial360Direction);
                break;
            case Image.FillMethod.Radial180:
                SerializedProperty radial180Direction = serializedObject.FindProperty("radial180Direction");
                EditorGUILayout.PropertyField(radial180Direction);
                break;
            case Image.FillMethod.Radial90:
                SerializedProperty radial90Direction = serializedObject.FindProperty("radial90Direction");
                EditorGUILayout.PropertyField(radial90Direction);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
