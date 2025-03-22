using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2.5f;
    public float crouchRunSpeed = 4f;

    public float crouchHeight = 1f;
    public float normalHeight = 1.8f;
    public float crouchCenter = 0.485f;
    public float standCenter = 0.97f;

    public Transform cameraTransform;
    public float standCamY = 1.6f;
    public float crouchCamY = 0.8f;

    public float maxStamina = 100f;
    public float stamina = 100f;
    public float sprintDrain = 20f;
    public float sprintRegen = 10f;

    private CharacterController controller;
    private float moveSpeed;
    private bool isCrouching = false;
    private bool isSprinting = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        moveSpeed = walkSpeed;
    }

    void Update()
    {
        HandleCrouch();
        MovePlayer();
        RegenerateStamina();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift) && moveZ > 0 && stamina > 0 && !isCrouching)
        {
            moveSpeed = runSpeed;
            stamina -= sprintDrain * Time.deltaTime;
            isSprinting = true;
        }
        else if (isCrouching && Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            moveSpeed = crouchRunSpeed;
            stamina -= sprintDrain * Time.deltaTime;
            isSprinting = true;
        }
        else
        {
            moveSpeed = isCrouching ? crouchSpeed : walkSpeed;
            isSprinting = false;
        }
    }

    void HandleCrouch()
    {
        bool isHoldingCrouch = Input.GetKey(KeyCode.LeftControl);
        isCrouching = isHoldingCrouch;

        float targetHeight = isCrouching ? crouchHeight : normalHeight;
        float targetCenter = isCrouching ? crouchCenter : standCenter;
        float targetCamY = isCrouching ? crouchCamY : standCamY;

        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * 10f);
        controller.center = new Vector3(controller.center.x, Mathf.Lerp(controller.center.y, targetCenter, Time.deltaTime * 10f), controller.center.z);

        Vector3 camPosition = cameraTransform.localPosition;
        camPosition.y = Mathf.Lerp(camPosition.y, targetCamY, Time.deltaTime * 10f);
        cameraTransform.localPosition = camPosition;
    }

    void RegenerateStamina()
    {
        if (!isSprinting)
        {
            stamina = Mathf.Min(stamina + sprintRegen * Time.deltaTime, maxStamina);
        }
    }
}
