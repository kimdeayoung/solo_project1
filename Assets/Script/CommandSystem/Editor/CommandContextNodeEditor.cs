using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CommandContextNode))]
public class CommandContextNodeEditor : Editor
{
    public sealed override void OnInspectorGUI()
    {
        serializedObject.Update();

        this.DrawScriptHeader();
        if (GUILayout.Button("Open Node Editor", GUILayout.Height(35)))
        {

        }

        serializedObject.ApplyModifiedProperties();
    }
}
