using UnityEngine;
using System.Collections.Generic;

/*
 *   Original Scrip from Valentijn Muijrers
 *   Edited by Ralf Zeilstra
 */

[System.Serializable]
public class Cell : MonoBehaviour
{
    public Vector2Int gridPosition;
    public Wall walls; //bit Encoded

    public bool playerStart;
    public GameObject WallPrefab;

    public void RemoveWall(Wall wallToRemove)
    {
        walls = (walls & ~wallToRemove);
    }

    /// <summary>
    /// Returns the amount of walls around the cell
    /// </summary>
    /// <returns></returns>
    public int GetNumWalls()
    {
        int numWalls = 0;
        if (((walls & Wall.DOWN) != 0)) { numWalls++; }
        if (((walls & Wall.UP) != 0)) { numWalls++; }
        if (((walls & Wall.LEFT) != 0)) { numWalls++; }
        if (((walls & Wall.RIGHT) != 0)) { numWalls++; }
        return numWalls;
    }

    public bool HasWall(Wall wallDirection)
    {
        return (walls & wallDirection) != 0;
    }

    /// <summary>
    /// Returns the neighbours of the cell
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    public List<Cell> GetNeighbours(Cell[,] grid)
    {
        List<Cell> result = new List<Cell>();
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                int cellX = this.gridPosition.x + x;
                int cellY = this.gridPosition.y + y;
                if (cellX < 0 || cellX >= grid.GetLength(0) || cellY < 0 || cellY >= grid.GetLength(1) || Mathf.Abs(x) == Mathf.Abs(y))
                {
                    continue;
                }
                Cell canditateCell = grid[cellX, cellY];
                result.Add(canditateCell);
            }
        }
        return result;
    }

    public Cell GetNorthernNeighbour(Cell[,] grid)
    {
        try { return grid[this.gridPosition.x, this.gridPosition.y + 1]; }
        catch { return null; }
    }

    public Cell GetEasternNeighbour(Cell[,] grid)
    {
        try { return grid[this.gridPosition.x + 1, this.gridPosition.y]; }
        catch { return null; }
    }

    public Cell GetSouthernNeighbour(Cell[,] grid)
    {
        try { return grid[this.gridPosition.x, this.gridPosition.y - 1]; }
        catch { return null; }
    }

    public Cell GetWesternNeighbour(Cell[,] grid)
    {
        try { return grid[this.gridPosition.x - 1, this.gridPosition.y]; }
        catch { return null; }
    }

    public void SpawnWalls(Cell cell)
    {
        if (cell.HasWall(Wall.DOWN)) { Instantiate(WallPrefab, transform.position, Quaternion.LookRotation(new Vector3(0, 0, -1)), transform); }
        if (cell.HasWall(Wall.UP)) { Instantiate(WallPrefab, transform.position, Quaternion.LookRotation(new Vector3(0, 0, 1)), transform); }
        if (cell.HasWall(Wall.LEFT)) { Instantiate(WallPrefab, transform.position, Quaternion.LookRotation(new Vector3(-1, 0, 0)), transform); }
        if (cell.HasWall(Wall.RIGHT)) { Instantiate(WallPrefab, transform.position, Quaternion.LookRotation(new Vector3(1, 0, 0)), transform); }
    }
}

[System.Flags]
public enum Wall
{
    LEFT = 1,
    UP = 2,
    RIGHT = 4,
    DOWN = 8
}
