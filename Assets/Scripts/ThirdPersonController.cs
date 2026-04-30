using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f; // velocidad de sprint
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;

    [Header("References")]
    public Transform cameraTransform;
    public Animator animator;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float turnSmoothVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        //mouse lock
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        


        // --- GROUND CHECK ---
        isGrounded = CheckGroundByTag();

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            animator.SetBool("isJumping", false);
        }

        // --- INPUT ---
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.wKey.isPressed) vertical += 1f;
        if (Keyboard.current.sKey.isPressed) vertical -= 1f;
        if (Keyboard.current.aKey.isPressed) horizontal -= 1f;
        if (Keyboard.current.dKey.isPressed) horizontal += 1f;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        bool isMoving = direction.magnitude >= 0.1f;
        bool shiftPressed = Keyboard.current.leftShiftKey.isPressed;

        //Sprint solo si te mueves + shift
        bool isRunning = isMoving && shiftPressed;

        //Si NO te mueves, sprint SIEMPRE debe ser false
        if (!isMoving)
            isRunning = false;

        // --- ANIMATIONS (ANTES DEL MOVIMIENTO) ---
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isRunning", isRunning);

        // --- MOVEMENT ---
        if (isMoving)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg
                                + cameraTransform.eulerAngles.y;

            float smoothAngle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref turnSmoothVelocity,
                0.1f
            );

            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            float speed = isRunning ? runSpeed : walkSpeed;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // --- JUMP ---
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("isJumping", true);
        }

        // --- FALLING ---
        animator.SetBool("isFalling", !isGrounded && velocity.y < 0);

        // --- GRAVITY ---
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    bool CheckGroundByTag()
    {
        Collider[] hits = Physics.OverlapSphere(groundCheck.position, groundDistance);

        foreach (Collider col in hits)
        {
            if (col.CompareTag("ground"))
                return true;
        }

        return false;
    }
}