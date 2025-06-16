using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarioController2 : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    int isWalkingHash = Animator.StringToHash("isWalking");
    int isRunningHash = Animator.StringToHash("isRunning");
    int isJumpingHash = Animator.StringToHash("isJumping");
    bool isJumpingAni;


    private Vector2 moveInput;
    private Vector3 moveDirection;

    private float speed = 4f;
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
    float minJumpDuration = 0.3f; 
    float jumpTimer = 0f;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        VariablesSaltos();
        rb.useGravity = false;
    }
    void VariablesSaltos()
    {
        float timeToApex = maxJumpTime / 2f;
        gravity = -2 * maxJumpHeight / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }
    void handleJump()
    {
        if (isJumpPressed && !isJumping && IsGrounded())
        {
            
            float jumpVelocity = initialJumpVelocity * 0.5f;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
            animator.SetBool(isJumpingHash, true);
            jumpTimer = 0f;
            isJumpingAni = true;
            isJumping = true;
        }
        else if (isJumping && !isJumpPressed && IsGrounded())
        {
            jumpTimer += Time.fixedDeltaTime;
            if (jumpTimer >= minJumpDuration)
            {
                isJumping = false;
            }
        }
    }
    void Update()
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
            }
            float groundedGravity = -0.5f;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, groundedGravity, rb.linearVelocity.z);
        }
        else if (isFalling)
        {
            float previousYVelocity = rb.linearVelocity.y;
            float newYVelocity = previousYVelocity + gravity * fallingMultiplier * Time.deltaTime;
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * 0.5f, -20f);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, nextYVelocity, rb.linearVelocity.z);
        }
        else
        {
            float previousYVelocity = rb.linearVelocity.y;
            float newYVelocity = previousYVelocity + gravity * Time.deltaTime;
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
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        isMoving = moveInput != Vector2.zero;
    }
    public void OnRun(InputAction.CallbackContext context)
    {
        isRunningPressed = context.ReadValueAsButton();
        speed = isRunningPressed ? 8f : 4f;
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
            Debug.Log("Jump Released");
        }
    }
    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
    }
}
