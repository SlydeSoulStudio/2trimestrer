using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 4f;
    public float mouseSensitivity = 100f;

    public float minY = -30f;
    public float maxY = 60f;

    float xRotation = 0f;
    float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime;
        float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minY, maxY);

        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        transform.position = target.position + offset;

        transform.LookAt(target);
    }
}