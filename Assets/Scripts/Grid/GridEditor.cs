using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(GameGrid))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //GUILayout.BeginHorizontal();

        //GameGrid grid = (GameGrid)target;
        //if (GUILayout.Button("Generate Grid"))
        //{
        //    grid.GenerateGrid();
        //}

        //if (GUILayout.Button("Delete Grid"))
        //{
        //    grid.DeleteGrid();
        //}

        //GUILayout.EndHorizontal();
    }
}
#endif