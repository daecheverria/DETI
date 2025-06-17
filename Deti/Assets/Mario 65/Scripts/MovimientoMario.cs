using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoMario : MonoBehaviour
{
    [Header("Configuracion de movimiento")]
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float rotationSpeed = 10f;

    private Vector2 moveInput;
    private bool isRunning;

    private Rigidbody rb;

    [Header("Configuracion de salto")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckDistance = 0.1f;
    [SerializeField] Transform groundCheck;
    private int jumpCount = 0;
    private bool isJumping = false;
    private float jumpStartTime;
    private float jumpVelocity;
    private float maxJumpTime = 5f;
    private float maxJumpHeight = 10f;
    Dictionary<int, float> jumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> jumpGravities = new Dictionary<int, float>();
    float gravity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        variablesSalto();
    }
    void variablesSalto()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow((timeToApex * 1.25f), 2) * 1.1f;
        jumpVelocity = (2 * (maxJumpHeight)) / (timeToApex * 1.25f);
        float secondJumpGravity = (-2 * maxJumpHeight + 10) / Mathf.Pow((timeToApex * 1.25f), 2) * 1.1f;
        float secondJumpInitialVelocity = (2 * (maxJumpHeight + 10))/ (timeToApex * 1.25f);
        float thirdJumpGravity = (-2 * maxJumpHeight + 20) / Mathf.Pow((timeToApex * 1.5f), 2) * 1.0f;
        float thirdJumpInitialVelocity = (2 * (maxJumpHeight + 20)) / (timeToApex * 1.5f);
        jumpVelocities.Add(1, jumpVelocity);
        jumpVelocities.Add(2, secondJumpInitialVelocity); 
        jumpVelocities.Add(3, thirdJumpInitialVelocity);}

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);
        float speed = isRunning ? runSpeed : walkSpeed;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && IsGrounded())
        {
            if(jumpCount >3){
                StopCoroutine("JumpReset");
            }
            isJumping = true;
            jumpCount += 1;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocities[jumpCount], rb.linearVelocity.z);
        }
    }
    IEnumerator JumpReset()
    {
        yield return new WaitForSeconds(0.25f);
        jumpCount = 0;
    }
    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
    }
}
