using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float sensitivity = 2f;  // Mouse sensitivity
    public Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Rotate camera up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 60f); // Prevent looking too far up/down
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate player left/right
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
