using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class GridCreationTool : EditorWindow
{
    [MenuItem("AI Tools/Grid Cration Tool")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow<GridCreationTool>();
    }

    private void OnGUI()
    {
        
    }
}
