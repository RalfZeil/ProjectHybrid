using NodeCanvas.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    //Behaviour
    private Blackboard bb;
    private bool foundPlayer;

    Astar astar;

    private float speed = 5f;
    private const float moveDelay = 1f;
    private float timer = 1f;

    [SerializeField] private Cell startCell;
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
        bb = GetComponent<Blackboard>();    
        astar = new Astar();

        currentCell = startCell;
        targetPos = currentCell.transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, speed * Time.deltaTime);

        if (bb.GetVariableValue<bool>("SeesPlayer"))
        {
            if (!foundPlayer)
            {
                FindNewPath(PlayerMovement.Instance.currentCell);
            }
            foundPlayer = true;
        }
        else
        {
            foundPlayer= false;
        }

        if(timer < 0)
        {
            if((path.Count > currentPathIndex))
            {
                SetNewDestination();
            }
            
            timer = moveDelay;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public void FindNewPath(Cell cell)
    {
        path = astar.FindPathToTarget(
            new Vector2Int(startCell.gridPosition.x, startCell.gridPosition.y), 
            new Vector2Int(cell.gridPosition.x, cell.gridPosition.y), 
            GameGrid.Instance.grid);
        currentPathIndex = 0;

        currentCell = GameGrid.Instance.GetCellWithPosition(path[currentPathIndex]);
    }

    private void SetNewDestination()
    {
        currentPathIndex++;
        currentCell = GameGrid.Instance.GetCellWithPosition(path[currentPathIndex]);
        targetPos = currentCell.transform.position;
    }
}
