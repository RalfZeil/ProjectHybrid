using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputActions playerInput;
    
    private float step = 5f;
    private float speed = 5f;

    private Vector3 targetPos;
    private Quaternion targetRot;

    private Cell currentCell;

    private void Start()
    {
        playerInput = new PlayerInputActions();
        playerInput.Enable();

        playerInput.Character.Walk.performed += ctx => Move(ctx.ReadValue<Vector2>());

        currentCell = Hybrid.Grid.Instance.GetPlayerStartCell();
        transform.position = currentCell.transform.position;
    }

    private void OnDestroy()
    {
        playerInput.Character.Walk.performed -= ctx => Move(ctx.ReadValue<Vector2>());
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, speed * Time.deltaTime);
    }

    private void Move(Vector2 input)
    {
        if (input.x > 0 || input.x < 0)
        {
            targetRot = Quaternion.Euler(0, targetRot.eulerAngles.y + (90 * input.x), 0);
        }
        else if (input.y > 0 || input.y < 0)
        {
            targetPos = transform.position + (transform.forward * step * input.y);
        }
    }
}
