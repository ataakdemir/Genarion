using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float mouseSensitivity = 5.0f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    public Vector3 targetOffset = new Vector3(0, 1.5f, 0); 

    private float xRot = 0.0f;
    private float yRot = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        xRot = angles.y;
        yRot = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        xRot += Input.GetAxis("Mouse X") * mouseSensitivity;
        yRot -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        yRot = Mathf.Clamp(yRot, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(yRot, xRot, 0);
        Vector3 offset = rotation * new Vector3(0.0f, 0.0f, -distance);

        Vector3 adjustedTargetPosition = target.position + targetOffset; 
        transform.position = adjustedTargetPosition + offset;
        transform.LookAt(adjustedTargetPosition);
    }
}
