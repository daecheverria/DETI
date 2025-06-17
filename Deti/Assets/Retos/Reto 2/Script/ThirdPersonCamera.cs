using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerOrientation;
    [SerializeField] private Transform combatLookAt;

    [Header("Camera Settings")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float cameraDistance = 5f;
    [SerializeField] private float minVerticalAngle = -20f;
    [SerializeField] private float maxVerticalAngle = 40f;
    [SerializeField] private bool invertY = false;
    [SerializeField] private bool invertX = false;

    [Header("Collision Settings")]
    [SerializeField] private LayerMask collisionLayers;
    [SerializeField] private float collisionOffset = 0.3f;
    [SerializeField] private float minCameraDistance = 1f;

    private float mouseX;
    private float mouseY;
    private float actualCameraDistance;
    private Vector3 cameraDirection;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        cameraDirection = transform.localPosition.normalized;
        actualCameraDistance = cameraDistance;
        
        // Si no hay orientación asignada, usar el transform del jugador
        if (playerOrientation == null)
        {
            playerOrientation = player;
        }
    }

    private void Update()
    {
        if (player == null) return;

        GetMouseInput();
    }

    private void LateUpdate()
    {
        if (player == null) return;

        HandleCameraRotation();
        HandleCameraCollision();
        UpdateCameraPosition();
    }

    private void GetMouseInput()
    {
        float invertXValue = invertX ? -1f : 1f;
        float invertYValue = invertY ? -1f : 1f;
        
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed * invertXValue;
        mouseY += Input.GetAxis("Mouse Y") * rotationSpeed * invertYValue;

        // Limitar el ángulo vertical de la cámara
        mouseY = Mathf.Clamp(mouseY, minVerticalAngle, maxVerticalAngle);
    }

    private void HandleCameraRotation()
    {
        // Rotar la orientación del jugador horizontalmente
        if (playerOrientation != null)
        {
            playerOrientation.rotation = Quaternion.Euler(0, mouseX, 0);
        }

        // Rotar la cámara verticalmente
        transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);
    }

    private void HandleCameraCollision()
    {
        Vector3 desiredCameraPos = player.position - (transform.rotation * Vector3.forward * cameraDistance);
        
        RaycastHit hit;
        if (Physics.Linecast(player.position, desiredCameraPos, out hit, collisionLayers))
        {
            actualCameraDistance = Mathf.Clamp(hit.distance - collisionOffset, minCameraDistance, cameraDistance);
        }
        else
        {
            actualCameraDistance = cameraDistance;
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 targetPosition = player.position - (transform.rotation * Vector3.forward * actualCameraDistance);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);
        
        // Si hay un punto de mira en combate, mirar hacia él
        if (combatLookAt != null)
        {
            transform.LookAt(combatLookAt);
        }
        else
        {
            transform.LookAt(player);
        }
    }

    public void SetCombatLookAt(Transform target)
    {
        combatLookAt = target;
    }
}