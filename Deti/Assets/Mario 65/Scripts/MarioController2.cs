using UnityEngine;
using UnityEngine.InputSystem;

public class MarioController2 : MonoBehaviour
{

    private Rigidbody rb;
    private Animator animator;

    int isWalkingHash = Animator.StringToHash("isWalking");
    int isRunningHash = Animator.StringToHash("isRunning");


    private Vector2 moveInput;
    private Vector3 moveDirection;
    
    private float speed = 2f;
    private bool isMoving;
    private bool isRunning;
    private bool isRunningPressed;
    private bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 2.0f;
    float maxJumpTime = 0.75f;
    bool isJumping = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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
        speed = isRunningPressed ? 5f : 2f;
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
}
