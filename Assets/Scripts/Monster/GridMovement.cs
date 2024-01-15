using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    Astar astar;

    private float speed = 5f;
    private float moveDelay = 1f;

    private Cell startCell;
    private Vector3 targetPos;
    private Quaternion targetRot;
    private float[] rotationAngles = new float[] { 0, 90, 180, 270 };
    private int currentRotationIndex = 0;

    private Cell prevCell;
    private Cell currentCell;

    private List<Vector2Int> path;
    private int currentPathIndex;

    private void Start()
    {
        astar = new Astar();

        currentCell = startCell;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, speed * Time.deltaTime);

        if((path.Count > currentPathIndex) && moveDelay < 0)
        {
            SetNewDestination();
        }
        else
        {
            moveDelay -= Time.deltaTime;
        }
    }

    public void FindNewPath(Cell cell)
    {
        path = astar.FindPathToTarget(
            new Vector2Int(startCell.gridPosition.x, startCell.gridPosition.y), 
            new Vector2Int(cell.gridPosition.x, cell.gridPosition.y), 
            GameGrid.Instance.grid);
        currentPathIndex = 0;

        currentCell = GameGrid.Instance.GetCellWithPosition(path[0]);
    }

    private void SetNewDestination()
    {
        currentPathIndex++;
        currentCell = GameGrid.Instance.GetCellWithPosition(path[currentPathIndex]);
    }
}
