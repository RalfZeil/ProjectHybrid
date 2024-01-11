using System;
using UnityEngine;
using UnityEngine.Windows;
using FMOD.Studio;
using UnityEditor.Callbacks;

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


    // sound related variables
    private Vector3 previousPosition;
    private float velocity;
    private EventInstance playerFootsteps;

    private void Start()
    {
        playerInput = new PlayerInputActions();
        playerInput.Enable();

        playerInput.Character.Move.performed += ctx => Move(ctx.ReadValue<float>());
        playerInput.Character.Rotate.performed += ctx => Rotate(ctx.ReadValue<float>());

        InitializePlayerPostition();

        // initialize footsteps
        playerFootsteps = AudioManager.instance.CreateEventInstance(FMODEvents.instance.playerFootsteps);
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

        UpdateSound();
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
        if (lastBeatTime + beatOffsetTime > Time.time) Debug.Log("Moved on beat!");

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

        if (prevCell != currentCell) { currentCell.OnEnterCell.Invoke(); }
    }

    public void SetBeatTime()
    {
        lastBeatTime = Time.time;
    }

    public void UpdateSound()
    {
        // if moving play footsteps
        Vector3 currentPosition = transform.position;
        if (previousPosition != null)
        {
            float distanceTravelled = Vector3.Distance(previousPosition, currentPosition);
            float timeTaken = Time.deltaTime;
            velocity = distanceTravelled / timeTaken;
        }
        previousPosition = currentPosition;

        if (velocity > 0.1f)
        {
            if (!playerFootsteps.isValid())
            {
                PLAYBACK_STATE playbackState;
                playerFootsteps.getPlaybackState(out playbackState);
                if (playbackState != PLAYBACK_STATE.PLAYING)
                {
                    playerFootsteps.start();
                }
            }
        }
        else
        {
            playerFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
