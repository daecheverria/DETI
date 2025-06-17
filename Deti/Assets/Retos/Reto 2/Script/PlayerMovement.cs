using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement2 : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float groundDrag = 5f;
    
    [Header("Mouse Look Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float maxLookAngle = 90f;
    [SerializeField] private bool invertY = false;

    [Header("References")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform orientation;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float horizontalInput;
    private float verticalInput;
    
    private float xRotation;
    private float yRotation;
    private float mouseX;
    private float mouseY;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
        // Bloquear y ocultar cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Configurar referencias si no están asignadas
        if (orientation == null) orientation = transform;
        if (playerCamera == null) playerCamera = Camera.main.transform;
    }

    private void Update()
    {
        GetInput();
        SpeedControl();
        HandleMouseLook();
        
        rb.linearDamping = groundDrag;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
    }

    private void MovePlayer()
    {
        // Calcular dirección de movimiento relativa a la orientación
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        // Aplicar fuerza en la dirección del movimiento
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
    
    private void HandleMouseLook()
    {
        // Rotación horizontal (izquierda/derecha)
        yRotation += mouseX;
        
        // Rotación vertical (arriba/abajo)
        float invert = invertY ? 1f : -1f;
        xRotation += mouseY * invert;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

        // Aplicar rotación a la cámara y al jugador
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}