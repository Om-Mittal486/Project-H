using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2.5f;
    public float crouchRunSpeed = 4f; // Slow run while crouching
    public float crouchHeight = 1f;
    public float normalHeight = 1.8f;
    public Transform cameraTransform; // Assign in Inspector
    public float standCamY = 1.6f;  // Default camera height
    public float crouchCamY = 0.8f; // Lower camera height

    private CharacterController controller;
    private float moveSpeed;

    public float maxStamina = 100f;
    public float stamina = 100f;
    public float sprintDrain = 20f;
    public float sprintRegen = 10f;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        moveSpeed = walkSpeed;
    }

    void Update()
    {
        MovePlayer();
        HandleCrouch();
        RegenerateStamina();
        MovePlayer();
        HandleCrouch();
    }

    void RegenerateStamina()
    {
        if (!Input.GetKey(KeyCode.LeftShift) || stamina <= 0)
        {
            stamina = Mathf.Min(stamina + sprintRegen * Time.deltaTime, maxStamina);
        }
    }
    void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            moveSpeed = (controller.height < normalHeight) ? crouchRunSpeed : runSpeed;
            stamina -= sprintDrain * Time.deltaTime;
        }
        else
        {
            moveSpeed = (controller.height < normalHeight) ? crouchSpeed : walkSpeed;
        }
    }

    void HandleCrouch()
    {
        bool isHoldingCrouch = Input.GetKey(KeyCode.LeftControl); // Check if holding Ctrl

        // Smooth height transition
        controller.height = Mathf.Lerp(controller.height, isHoldingCrouch ? crouchHeight : normalHeight, Time.deltaTime * 10f);

        // Smooth camera transition
        Vector3 camPosition = cameraTransform.localPosition;
        camPosition.y = Mathf.Lerp(camPosition.y, isHoldingCrouch ? crouchCamY : standCamY, Time.deltaTime * 10f);
        cameraTransform.localPosition = camPosition;
    }
}
