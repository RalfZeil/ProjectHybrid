using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static GameGrid Instance;

    [Header("Grid settings")]
    public int width = 20;
    public int height = 20;
    public Cell[,] grid;
    public float scaleFactor = 1f;
    public GameObject cellPrefab;

    private List<Cell> allCellObjects = new List<Cell>();

    private void Awake()
    {
        Instance = this;
        InitializeGridAndList();
    }

    private void InitializeGridAndList()
    {
        grid = new Cell[width, height];
        allCellObjects = new List<Cell>();

        foreach (Transform child in transform)
        {
            Cell cell = child.GetComponent<Cell>();

            if (cell != null)
            {
                grid[cell.gridPosition.x, cell.gridPosition.y] = cell;
                allCellObjects.Add(cell);
            }
        }
    }

    public Cell GetCellWithPosition(Vector2Int pos)
    {
        return grid[pos.x, pos.y];
    }

    /// <summary>
    /// Returns the first cell with the PlayerStart bool thats true
    /// </summary>
    /// <returns></returns>
    public Cell GetPlayerStartCell()
    {
        foreach (Cell cell in allCellObjects)
        {
            if (cell.playerStart == true) return cell;
        }

        Debug.LogWarning("There is no Player Start defined, check atleast one cell to Player Start");
        return null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        grid = new Cell[width, height];
        allCellObjects = new List<Cell>();

        foreach (Transform child in transform)
        {
            Cell cell = child.GetComponent<Cell>();

            if (cell != null)
            {
                grid[cell.gridPosition.x, cell.gridPosition.y] = cell;
                allCellObjects.Add(cell);
            }
        }

        Gizmos.color = Color.green;

        foreach (Cell cell in allCellObjects)
        {
            List<Cell> neighbours = cell.GetNeighbours(grid);

            foreach (Cell neighbour in neighbours)
            {
                Gizmos.DrawLine(cell.transform.position, neighbour.transform.position);
            }
        }
    }

    public void GenerateGrid()
    {
        grid = new Cell[width, height];
        grid.Initialize();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell newCell = Instantiate(cellPrefab, new Vector3(x * scaleFactor, 0, y * scaleFactor), Quaternion.identity, transform).GetComponent<Cell>();
                grid[x, y] = newCell;
                grid[x, y].gridPosition = new Vector2Int(x, y);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y].SpawnWalls(grid[x, y]);
            }
        }
    }

    public void DeleteGrid()
    {
        foreach (Transform transform in transform)
        {
            DestroyImmediate(transform.gameObject);
        }
    }
#endif
}
