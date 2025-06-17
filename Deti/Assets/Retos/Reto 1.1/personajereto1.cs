using UnityEngine;
using UnityEngine.InputSystem;

public class personajereto1 : MonoBehaviour
{
    CharacterController controller;
    float speed = 5f;
    Vector3 moveDirection;
    private float fallSpeed = 0f;
    private float gravity = 9.81f;
    private float dashCooldown = 1f;
    private float dashDuration = 0.5f;
    private bool canDash = true;
    private bool isDashing = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.performed)
            {
                Vector2 moveInput = context.ReadValue<Vector2>();
                moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
            }
        }
        if (context.canceled)
        {
            moveDirection = Vector3.zero;
        }

    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            speed = 10f;
        }else if (context.canceled)
        {
            speed = 5f;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && IsGrounded() && !isDashing){
            isDashing = true;
            canDash = false;
            speed = 20f;
            Invoke("ResetDash", dashDuration);
        }
    }
    void ResetDash()
    {
        isDashing = false;
        speed = 5f;
        Invoke("ResetDashCooldown", dashCooldown);
    }
    void ResetDashCooldown()
    {
        canDash = true;
    }
    void FixedUpdate()
    {
        if (!IsGrounded())
        {
            fallSpeed += gravity * Time.deltaTime;
            moveDirection.y -= fallSpeed * Time.deltaTime;
        }
        else
        {
            fallSpeed = 0f;
            moveDirection.y = 0f;
        }
        controller.Move(moveDirection * speed * Time.deltaTime);

    }
    bool IsGrounded()
    {
        return controller.isGrounded;
    }
}
