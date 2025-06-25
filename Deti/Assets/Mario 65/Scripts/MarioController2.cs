using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarioController2 : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] AudioClip[] jumpSounds;
    [SerializeField] private Transform cameraTransform; // Referencia a la transformación de la cámara

    int isWalkingHash = Animator.StringToHash("isWalking");
    int isRunningHash = Animator.StringToHash("isRunning");
    int isJumpingHash = Animator.StringToHash("isJumping");
    int jumpCountHash = Animator.StringToHash("jumpCount");
    bool isJumpingAni;

    private Vector2 moveInput;
    private Vector3 moveDirection;

    [SerializeField] private float basespeed = 8f;
    private float speed;

    private bool isMoving;
    private bool isRunning;
    private bool isRunningPressed;
    private bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 6.0f;
    float maxJumpTime = 0.75f;
    bool isJumping = false;
    float gravity = -9.81f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckDistance = 0.1f;
    [SerializeField] Transform groundCheck;
    int jumpCount = 0;
    Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> jumpGravities = new Dictionary<int, float>();
    Coroutine currentJumpCoroutine = null;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        VariablesSaltos();
        rb.useGravity = false;
        speed = basespeed;
        audioSource = GetComponent<AudioSource>();
        
        // Si no se asigna manualmente, intenta encontrar la cámara principal
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }
    void VariablesSaltos()
    {
        float timeToApex = maxJumpTime / 2f;
        gravity = -2 * maxJumpHeight / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        float secondJumpGravity = (-2 * (maxJumpHeight*2.0f)) / Mathf.Pow((timeToApex * 1.25f), 2);
        float secondJumpInitialVelocity = (2 * (maxJumpHeight*2.0f)) / (timeToApex * 1.25f);
        float thirdJumpGravity = (-2 * (maxJumpHeight*3.0f)) / Mathf.Pow((timeToApex * 1.5f), 2);
        float thirdJumpInitialVelocity = (2 * (maxJumpHeight*3.0f)) / (timeToApex * 1.5f);


        initialJumpVelocities.Add(1, initialJumpVelocity);
        initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        initialJumpVelocities.Add(3, thirdJumpInitialVelocity);


        jumpGravities.Add(0, gravity);
        jumpGravities.Add(1, gravity);
        jumpGravities.Add(2, secondJumpGravity);
        jumpGravities.Add(3, thirdJumpGravity);

    }
    void handleJump()
    {
        if (isJumpPressed && !isJumping && IsGrounded() && jumpCount < 3)
        {
            if (jumpCount < 3 && currentJumpCoroutine != null)
            {
                StopCoroutine(currentJumpCoroutine);
            }
            animator.SetBool(isJumpingHash, true);
            isJumpingAni = true;
            isJumping = true;
            jumpCount++;
            animator.SetInteger(jumpCountHash, jumpCount);
            audioSource.PlayOneShot(jumpSounds[jumpCount - 1]);
            float jumpVelocity = initialJumpVelocities[jumpCount] * 0.5f;
            Debug.Log("Salto aplicado");
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
            //rb.AddForce(new Vector3(0, initialJumpVelocities[jumpCount]/2, 0), ForceMode.Impulse);
        }
        else if (isJumping && !isJumpPressed && IsGrounded())
        {
            isJumping = false;
        }
    }
    IEnumerator JumpReset()
    {
        yield return new WaitForSeconds(0.5f);
        jumpCount = 0;
    }
    void FixedUpdate()
    {
        handleAnimation();
        rb.MovePosition(rb.position + moveDirection * speed * Time.deltaTime);
        if (isMoving)
        {
            Vector3 rotation;
            rotation.x = moveDirection.x;
            rotation.y = 0;
            rotation.z = moveDirection.z;
            Quaternion targetRotation = Quaternion.LookRotation(rotation);
            rb.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

        }
        handleGravity();
        handleJump();
    }
    void handleGravity()
    {
        bool isFalling = rb.linearVelocity.y < 0 || !isJumpPressed;
        float fallingMultiplier = 2.0f;

        if (IsGrounded())
        {
            if (isJumpingAni)
            {
                animator.SetBool(isJumpingHash, false);
                isJumpingAni = false;
                currentJumpCoroutine = StartCoroutine(JumpReset());
                if (jumpCount == 3)
                {
                    jumpCount = 0;
                    animator.SetInteger(jumpCountHash, jumpCount);
                }
            }
            float groundedGravity = -0.5f;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, groundedGravity, rb.linearVelocity.z);
        }
        else if (isFalling)
        {
            float previousYVelocity = rb.linearVelocity.y;
            float newYVelocity = previousYVelocity + jumpGravities[jumpCount] * fallingMultiplier * Time.deltaTime;
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * 0.5f, -20f);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, nextYVelocity, rb.linearVelocity.z);
        }
        else
        {
            float previousYVelocity = rb.linearVelocity.y;
            float newYVelocity = previousYVelocity + jumpGravities[jumpCount] * Time.deltaTime;
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, nextYVelocity, rb.linearVelocity.z);
        }
    }
    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        if (isMoving && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if (!isMoving && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }
        if (isMoving && isRunningPressed && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if ((!isMoving || !isRunningPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }


    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        
        // Calcular la dirección de movimiento relativa a la cámara
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        
        // Ignorar la componente Y para movimiento horizontal
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        
        // Combinar las direcciones con el input
        moveDirection = forward * moveInput.y + right * moveInput.x;
        
        isMoving = moveInput != Vector2.zero;
    }
    public void OnRun(InputAction.CallbackContext context)
    {
        isRunningPressed = context.ReadValueAsButton();
        speed = isRunningPressed ? basespeed*2 : basespeed;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isJumpPressed = true;
        }
        else if (context.canceled)
        {
            isJumpPressed = false;
        }
    }
    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
    }
}
