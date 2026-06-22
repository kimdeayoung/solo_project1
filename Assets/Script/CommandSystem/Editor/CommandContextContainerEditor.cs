using UnityEditor;

[CustomEditor(typeof(CommandContextContainer))]
public partial class CommandContextContainerEditor : Editor
{
    public sealed override void OnInspectorGUI()
    {
        serializedObject.Update();

        this.DrawScriptHeader();
        this.DrawProperty("identifier");

        this.DrawProperty("resetFlag");

        this.DrawProperty("rootNode");
    }
}
