using UnityEngine;

public class RigidbodyJugador : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private Vector3 direction;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Detectar input de movimiento
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Detectar salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Saltar();
        }

        // Verificar si est� en el suelo
        VerificarSuelo();
    }

    void FixedUpdate()
    {
        // Aplicar fuerza de movimiento
        if (direction.magnitude >= 0.1f)
        {
            rb.AddForce(direction * acceleration, ForceMode.Acceleration);

            // Limitar velocidad m�xima
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            if (horizontalVelocity.magnitude > maxSpeed)
            {
                horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
                rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
            }
        }
    }

    void VerificarSuelo()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer);
    }

    void Saltar()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Plataforma de salto
        if (collision.gameObject.CompareTag("JumpPlatform"))
        {
            rb.AddForce(Vector3.up * jumpForce * 1.5f, ForceMode.Impulse);
        }
    }
}