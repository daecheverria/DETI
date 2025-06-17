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

    //saltos
    private bool isGrounded = true;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] LayerMask groundLayer; // Capa del suelo
    [SerializeField] float groundCheckDistance = 0.1f; // Distancia para verificar si está en el suelo
    [SerializeField] Transform groundCheck; // Transform para verificar el suelo
    private int jumpCount = 0; // Contador de saltos

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Estas se conectan desde el Inspector
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
        if (context.performed)
        {
            if (IsGrounded())
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }


    }
    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
    }
}
