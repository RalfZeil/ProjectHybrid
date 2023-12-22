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
        public GameObject cellPrefab;

        private List<Cell> allCellObjects = new List<Cell>();

        public void GenerateGrid()
        {
            grid = new Cell[width, height];
            grid.Initialize();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = Instantiate(cellPrefab, new Vector3(x * scaleFactor, 0, y * scaleFactor), Quaternion.identity, transform).GetComponent<Cell>();
                    grid[x, y].gridPosition = new Vector2Int(x, y);
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y].SpawnWalls(grid[x, y]);
                    allCellObjects.Add(grid[x, y]);
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
            return null;
        }
    }
}
