using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float sprintSpeed = 10f;   // súbelo para notar bien la diferencia
    public float rotationSpeed = 120f;

    [Header("Jump Settings")]
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private float groundHorizontalSpeed;   // velocidad mientras estás en suelo
    private float jumpHorizontalSpeed;     // velocidad que se mantiene en el aire

    void Start()
    {
        controller = GetComponent<CharacterController>();
        groundHorizontalSpeed = moveSpeed;
        jumpHorizontalSpeed = moveSpeed;
    }

    void Update()
    {
        // --- GROUND CHECK ---
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // --- INPUT ---
        float move = 0f;
        float rotation = 0f;

        if (Keyboard.current.wKey.isPressed) move = 1;
        if (Keyboard.current.sKey.isPressed) move = -1;

        if (Keyboard.current.aKey.isPressed) rotation = -1;
        if (Keyboard.current.dKey.isPressed) rotation = 1;

        transform.Rotate(Vector3.up * rotation * rotationSpeed * Time.deltaTime);

        // --- SPRINT EN SUELO ---
        if (isGrounded)
        {
            bool sprinting = Keyboard.current.leftShiftKey.isPressed && move > 0;
            groundHorizontalSpeed = sprinting ? sprintSpeed : moveSpeed;
        }

        // --- JUMP ---
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // guardamos la velocidad horizontal del momento del salto
            jumpHorizontalSpeed = groundHorizontalSpeed;
        }

        // --- MOVIMIENTO HORIZONTAL ---
        float usedSpeed = isGrounded ? groundHorizontalSpeed : jumpHorizontalSpeed;
        Vector3 moveDir = transform.forward * move * usedSpeed;

        // --- GRAVEDAD ---
        velocity.y += gravity * Time.deltaTime;
        moveDir.y = velocity.y;

        // --- MOVE ---
        controller.Move(moveDir * Time.deltaTime);
    }
}
