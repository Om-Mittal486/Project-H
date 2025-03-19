using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2.5f;
    public float crouchRunSpeed = 4f;
    public float crouchHeight = 1f;
    public float normalHeight = 1.8f;
    public Transform cameraTransform; // Assign in Inspector
    public float standCamY = 1.6f;
    public float crouchCamY = 0.8f;

    public float jumpHeight = 1.2f; // Lowered for more natural jump
    public float gravity = -20f; // Increased gravity for faster fall

    private CharacterController controller;
    private float moveSpeed;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        moveSpeed = walkSpeed;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Prevent floating when touching the ground
        }

        MovePlayer();
        HandleCrouch();
        HandleJump();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Running while standing or crouching
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = (controller.height < normalHeight) ? crouchRunSpeed : runSpeed;
        }
        else
        {
            moveSpeed = (controller.height < normalHeight) ? crouchSpeed : walkSpeed;
        }
    }

    void HandleCrouch()
    {
        bool isHoldingCrouch = Input.GetKey(KeyCode.LeftControl);

        // Smooth height transition
        controller.height = Mathf.Lerp(controller.height, isHoldingCrouch ? crouchHeight : normalHeight, Time.deltaTime * 10f);

        // Smooth camera transition
        Vector3 camPosition = cameraTransform.localPosition;
        camPosition.y = Mathf.Lerp(camPosition.y, isHoldingCrouch ? crouchCamY : standCamY, Time.deltaTime * 10f);
        cameraTransform.localPosition = camPosition;
    }

    void HandleJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Jump force
        }

        velocity.y += gravity * Time.deltaTime; // Apply gravity
        controller.Move(velocity * Time.deltaTime);
    }
}
