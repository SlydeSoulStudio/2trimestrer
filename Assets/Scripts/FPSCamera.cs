using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class FPSCamera : MonoBehaviour
{
    public float mouseSense = 100f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * mouseSense * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSense * Time.deltaTime;

        //vertical movement only camera
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //horizontal movement
        playerBody.Rotate(Vector3.up * mouseX);
    }
}