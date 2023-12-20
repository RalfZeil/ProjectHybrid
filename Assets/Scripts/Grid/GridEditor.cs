using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Hybrid.Grid grid = (Hybrid.Grid)target;
        if (GUILayout.Button("Generate Grid"))
        {
            grid.GenerateGrid();
        }
    }
}
#endif
