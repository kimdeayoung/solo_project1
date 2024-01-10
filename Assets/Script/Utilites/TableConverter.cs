using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR

using UnityEditor;
public class TableConverter : EditorWindow
{
    private static readonly string TableConvertBtnName = "Table Convert";

    [MenuItem("Utilities/TableConverter")]
    private static void Init()
    {
        TableConverter editorWindow = (TableConverter)GetWindow(typeof(TableConverter));
        editorWindow.Show();
    }

    private static void TableConvert()
    {
        Tables.Instance.ConvertLocalTableToBinary();
    }

    public void OnGUI()
    {
        if (GUILayout.Button(TableConvertBtnName))
        {
            TableConvert();
        }
    }
}
#endif
