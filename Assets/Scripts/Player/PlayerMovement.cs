using System;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputActions playerInput;

    private float speed = 5f;

    private Vector3 targetPos;
    private Quaternion targetRot;
    private float[] rotationAngles = new float[] { 0, 90, 180, 270 };
    private int currentRotationIndex = 0;

    private Cell prevCell;
    private Cell currentCell;

    //beatsync
    public float beatOffsetTime;
    private float lastBeatTime;

    private void Start()
    {
        playerInput = new PlayerInputActions();
        playerInput.Enable();

        playerInput.Character.Move.performed += ctx => Move(ctx.ReadValue<float>());
        playerInput.Character.Rotate.performed += ctx => Rotate(ctx.ReadValue<float>());

        InitializePlayerPostition();
    }

    private void InitializePlayerPostition()
    {
        SetNewDestination(currentCell = GameGrid.Instance.GetPlayerStartCell());
    }

    private void OnDestroy()
    {
        playerInput.Disable();

        playerInput.Character.Move.performed -= ctx => Move(ctx.ReadValue<float>());
        playerInput.Character.Rotate.performed -= ctx => Rotate(ctx.ReadValue<float>());
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, speed * Time.deltaTime);
    }

    private void HandleInput(Vector2 input)
    {
        if (input.x != 0)
        {
            Rotate(input.x);
        }
    }

    public void SetNewDestination(Cell cell)
    {
        if (cell == null) { return; }
        currentCell = cell;
        targetPos = cell.transform.position;
    }

    private void Rotate(float inputX)
    {
        // Update the rotation index
        currentRotationIndex += (inputX > 0) ? 1 : -1;

        // Wrap the index to stay within the array bounds
        if (currentRotationIndex < 0) currentRotationIndex = rotationAngles.Length - 1;
        if (currentRotationIndex >= rotationAngles.Length) currentRotationIndex = 0;

        // Set the target rotation from the array
        targetRot = Quaternion.Euler(0, rotationAngles[currentRotationIndex], 0);
    }

    private void Move(float inputY)
    {
        if(lastBeatTime + beatOffsetTime > Time.time) Debug.Log("Moved on beat!");

        prevCell = currentCell;

        if (inputY > 0) // Move forward TODO: Move forward instead of always going to a specific wind direction
        {
            switch (currentRotationIndex)
            {
                case 0:
                    SetNewDestination(currentCell.GetNorthernNeighbour(GameGrid.Instance.grid));
                    break;
                case 1:
                    SetNewDestination(currentCell.GetEasternNeighbour(GameGrid.Instance.grid));
                    break;
                case 2:
                    SetNewDestination(currentCell.GetSouthernNeighbour(GameGrid.Instance.grid));
                    break;
                case 3:
                    SetNewDestination(currentCell.GetWesternNeighbour(GameGrid.Instance.grid));
                    break;
            }

            currentCell.OnEnterCell.Invoke();
        }
        else if (inputY < 0) // Move Backward
        {
            switch (currentRotationIndex)
            {
                case 0:
                    SetNewDestination(currentCell.GetSouthernNeighbour(GameGrid.Instance.grid));
                    break;
                case 1:
                    SetNewDestination(currentCell.GetWesternNeighbour(GameGrid.Instance.grid));
                    break;
                case 2:
                    SetNewDestination(currentCell.GetNorthernNeighbour(GameGrid.Instance.grid));
                    break;
                case 3:
                    SetNewDestination(currentCell.GetEasternNeighbour(GameGrid.Instance.grid));
                    break;
            }
        }

        if(prevCell != currentCell) { currentCell.OnEnterCell.Invoke(); }
    }

    public void SetBeatTime(){
        lastBeatTime = Time.time;
    }
}
