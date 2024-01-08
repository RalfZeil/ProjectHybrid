using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputActions playerInput;

    private float step = 5f;
    private float speed = 5f;

    private Vector3 targetPos;
    private Quaternion targetRot;
    private float[] rotationAngles = new float[] { 0, 90, 180, 270 };
    private int currentRotationIndex = 0;
    private Cell currentCell;

    private void Start()
    {
        playerInput = new PlayerInputActions();
        playerInput.Enable();

        playerInput.Character.Move.performed += ctx => Move(ctx.ReadValue<float>());
        playerInput.Character.Rotate.performed += ctx => Rotate(ctx.ReadValue<float>());

        currentCell = Hybrid.Grid.Instance.GetPlayerStartCell();
        transform.position = currentCell.transform.position;
    }

    private void OnDestroy()
    {
        playerInput.Disable();
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
        if (input.y != 0)
        {
            Move(input.y);
        }
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
        // Calculate the forward direction based on targetRot
        Vector3 direction = Quaternion.Euler(0, rotationAngles[currentRotationIndex], 0) * Vector3.forward;

        // Update the target position for movement
        targetPos = transform.position + (direction * step * inputY);
    }
}
