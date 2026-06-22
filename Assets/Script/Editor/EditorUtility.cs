using UnityEditor;
using UnityEngine;

public static class EditorUtility
{
    public static void DrawScriptHeader(this Editor editor)
    {
        editor.DrawProperty("m_Script");
    }

    public static SerializedProperty FindProperty(this Editor editor, string propertyPath)
    {
        return editor.serializedObject.FindProperty(propertyPath);
    }

    public static void DrawProperty(this Editor editor, string propertyPath, bool includeChildren = true)
    {
        SerializedProperty property = editor.serializedObject.FindProperty(propertyPath);
        EditorGUILayout.PropertyField(property, includeChildren);
    }
}
