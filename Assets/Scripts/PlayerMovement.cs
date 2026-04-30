using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float sprintMultiplier = 1.5f;
    public float jumpHeight = 1.0f;
    public float gravity = -9.81f;
    public float rotationSpeed = 120f;
    public float backwardSpeedMultiplier = 0.5f;
    public float airControlMultiplier = 0.5f;

    private Vector3 velocity;

    private CharacterController controller;

    private bool onGround;
    private bool wasGroundedLastFrame;
    public bool movementLocked = false;

    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    // --- ANIMATOR ---
    private Animator anim;

    // --- SOUND ---
    public AudioClip jumpClip;
    public AudioClip landClip;
    private AudioSource audioSource;

    //remember if jump was with sprint
    private bool sprintJump;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // -- Bloquea y pone las animaciones en 0 cuando esta muerto --
        PlayerHealth health = GetComponent<PlayerHealth>();
        if (health != null && health.isDead)
        {
            anim.SetFloat("Speed", 0);
            anim.SetFloat("MoveX", 0);
            anim.SetFloat("MoveY", 0);
            anim.SetFloat("VerticalVelocity", 0);

            anim.SetBool("isRunning", false);
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
            anim.SetBool("isRolling", false);
            anim.SetBool("isGrounded", true);

            return;
        }

        // -- final del bloque de muerte --

        if (movementLocked) return;

        // --- GROUND CHECK ---
        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // --- FALL INSTANTÁNEA AL SALIR DEL BORDE ANDANDO ---
        if (!onGround && wasGroundedLastFrame && !anim.GetBool("isJumping"))
        {
            anim.SetBool("isFalling", true);
        }

        // --- FIX: evita el sonido al empezar ---
        if (Time.frameCount == 1)
            wasGroundedLastFrame = onGround;

        // --- LANDING SOUND ---
        if (onGround && !wasGroundedLastFrame)
        {
            if (velocity.y < -4f && landClip != null)
                audioSource.PlayOneShot(landClip);
        }

        // --- INPUT MOVIMIENTO ---
        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            input.y += 1;
        if (Keyboard.current.sKey.isPressed)
            input.y -= 1;

        if (Keyboard.current.dKey.isPressed)
            input.x += 1;
        if (Keyboard.current.aKey.isPressed)
            input.x -= 1;

        // Sprint según input
        bool sprintInput = Keyboard.current.leftShiftKey.isPressed && input.y > 0;

        // --- ANIMACIONES DE DIRECCIÓN ---
        anim.SetFloat("MoveX", input.x, 0.1f, Time.deltaTime);
        anim.SetFloat("MoveY", input.y, 0.1f, Time.deltaTime);

        // --- Sprint Animation ---
        bool runningAnim = sprintInput && onGround;
        anim.SetBool("isRunning", runningAnim);

        // --- ROTACIÓN IZQUIERDA DERECHA (A/D) ---
        if (input.x != 0)
        {
            transform.Rotate(Vector3.up * input.x * rotationSpeed * Time.deltaTime);
        }

        // --- MOVIMIENTO ADELANTE ATRÁS (W/S) ---
        float finalSpeed = moveSpeed;

        if (sprintInput && onGround)
            finalSpeed *= sprintMultiplier;

        if (input.y < 0)
            finalSpeed *= backwardSpeedMultiplier;

        // --- CONTROL AÉREO ---
        float appliedSpeed = onGround ? finalSpeed : finalSpeed * airControlMultiplier;

        // Sprint Jump en aire
        if (!onGround && sprintJump)
        {
            if (input.y > 0)
                appliedSpeed = moveSpeed * sprintMultiplier;
            else
                appliedSpeed = moveSpeed * airControlMultiplier;
        }

        // movimiento horizontal
        Vector3 move = transform.forward * input.y * appliedSpeed;

        // --- GRAVEDAD CLÁSICA (no pisa el salto) ---
        if (onGround && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        move.y = velocity.y;

        controller.Move(move * Time.deltaTime);

        // --- ANIMACIÓN DE MOVIMIENTO ---
        float currentSpeed = new Vector2(move.x, move.z).magnitude;
        anim.SetFloat("Speed", currentSpeed);

        // --- ANIMACIÓN: GROUND ---
        anim.SetBool("isGrounded", onGround);

        // --- JUMP ---
        if (Keyboard.current.spaceKey.wasPressedThisFrame && onGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetBool("isJumping", true);

            sprintJump = sprintInput;

            if (jumpClip != null)
                audioSource.PlayOneShot(jumpClip);
        }

        // --- FALL DETECTION ---
        if (!onGround && velocity.y < -9f)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
        }

        // mirar si esta en sprint
        if (!onGround && wasGroundedLastFrame)
        {
            sprintJump = Keyboard.current.leftShiftKey.isPressed;
        }

        // --- LANDING (animación) ---
        if (onGround && !wasGroundedLastFrame)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
            sprintJump = false;
        }

        // Evitar quedarse encima de enemigos
        Vector3 rayOrigin = transform.position + Vector3.up * 1.5f;
        int mask = ~LayerMask.GetMask("Player");

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 3f, mask))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                velocity.y = -10f;
                onGround = false;
            }
        }

        // --- ROLL (rodar) ---
        anim.SetBool("isRolling", Keyboard.current.cKey.wasPressedThisFrame && onGround);

        // --- SAVE GROUND STATE ---
        wasGroundedLastFrame = onGround;

        // --- ANIMACIÓN: VELOCIDAD VERTICAL ---
        anim.SetFloat("VerticalVelocity", velocity.y);
    }
}
