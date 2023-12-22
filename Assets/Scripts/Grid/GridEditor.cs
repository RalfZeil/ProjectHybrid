using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(Hybrid.Grid))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.BeginHorizontal();

        Hybrid.Grid grid = (Hybrid.Grid)target;
        if (GUILayout.Button("Generate Grid"))
        {
            grid.GenerateGrid();
        }

        if (GUILayout.Button("Delete Grid"))
        {
            grid.DeleteGrid();
        }

        GUILayout.EndHorizontal();
    }
}
#endif