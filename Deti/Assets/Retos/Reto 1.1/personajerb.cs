using UnityEngine;
using UnityEngine.InputSystem;

public class personajerb : MonoBehaviour
{
    Rigidbody rb;
    float speed = 5f;
    Vector3 moveDirection;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f; // Distancia para verificar si está en el suelo

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    void FixedUpdate()
    {
        //rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
        rb.AddForce(moveDirection * acceleration, ForceMode.Acceleration);
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

}
