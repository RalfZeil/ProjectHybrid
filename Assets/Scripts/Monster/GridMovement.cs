using NodeCanvas.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridMovement : MonoBehaviour
{
    public UnityEvent OnCollideWithPlayer;

    //Behaviour
    private Blackboard bb;
    private bool foundPlayer;

    Astar astar;

    private float speed = 5f;
    private const float moveDelayInSeconds = 2f;
    private float timer = 1f;

    [SerializeField] private Cell startCell;
    private Vector3 targetPos;
    private Quaternion targetRot;

    private Cell prevCell;
    private Cell currentCell;

    private Queue<Vector2Int> path = new Queue<Vector2Int>();

    private void Start()
    {
        bb = GetComponent<Blackboard>();    
        astar = new Astar();

        currentCell = startCell;
        targetPos = currentCell.transform.position;
    }

    private void Update()
    {
        if (path.Count > 0) { Debug.Log(path.Peek()); }

        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, speed * Time.deltaTime);

        if (bb.GetVariableValue<bool>("SeesPlayer"))
        {
            if (!foundPlayer)
            {
                
            }
            foundPlayer = true;
        }
        else
        {
            foundPlayer= false;
        }

        if(timer < 0)
        {
            FindNewPath(PlayerMovement.Instance.currentCell);
            if ((path.Count > 0))
            {
                SetNewDestination();
            }
            else if(foundPlayer)
            {
                FindNewPath(PlayerMovement.Instance.currentCell);
            }
            
            timer = moveDelayInSeconds;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public void FindNewPath(Cell cell)
    {
        path = new Queue<Vector2Int>(astar.FindPathToTarget(
            new Vector2Int(currentCell.gridPosition.x, currentCell.gridPosition.y),
            new Vector2Int(cell.gridPosition.x, cell.gridPosition.y),
            GameGrid.Instance.grid));

        currentCell = GameGrid.Instance.GetCellWithPosition(path.Dequeue());
    }

    private void SetNewDestination()
    {
        currentCell = GameGrid.Instance.GetCellWithPosition(path.Dequeue());
        targetPos = currentCell.transform.position;

        Vector3 direction = targetPos - transform.position;
        direction.y = 0; 

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        targetRot = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
    }

    public void OverrideCurrentPositionWithCell(Cell cell)
    {
        currentCell = cell;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if it collides with player
        if(other.GetComponent<PlayerMovement>() != null) 
        {
            Debug.Log("Collided with Player");
            OnCollideWithPlayer.Invoke();
        }
    }
}
