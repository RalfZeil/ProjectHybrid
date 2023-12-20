using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hybrid
{
    public class Grid : MonoBehaviour
    {
        [Header("Grid settings")]
        public int width = 10;
        public int height = 10;
        public Cell[,] grid;
        public float scaleFactor = 1f;
        public CellPrefab cellPrefab;

        private List<GameObject> allCellObjects = new List<GameObject>();

        public void GenerateGrid()
        {
            grid = new Cell[width, height];
            grid.Initialize();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = new Cell();
                    grid[x, y].gridPosition = new Vector2Int(x, y);
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    CellPrefab cellObject = Instantiate(cellPrefab, new Vector3(x * scaleFactor, 0, y * scaleFactor), Quaternion.identity, transform);
                    cellObject.SpawnWalls(grid[x, y]);
                    allCellObjects.Add(cellObject.gameObject);
                }
            }
        }
    }
}
