using UnityEngine;
public class EnemyFollow : MonoBehaviour
{
    // =============================================
    // Configuración de movimiento (visible en el Inspector)
    // =============================================
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;          // Velocidad de movimiento del enemigo
    [SerializeField] private float rotationSpeed = 5f;     // Velocidad de rotación hacia el jugador
    [SerializeField] private float stoppingDistance = 1f;  // Distancia mínima para detenerse
    [SerializeField] private float detectionRange = 10f;   // Rango de detección del jugador
    
    // =============================================
    // Referencias a otros componentes/objetos
    // =============================================
    [Header("References")]
    [SerializeField] private Transform player;  // Referencia al transform del jugador
    [SerializeField] private Rigidbody rb;     // Componente Rigidbody para física

    // =============================================
    // Inicialización (se ejecuta al cargar el objeto)
    // =============================================
    private void Awake()
    {
        // Busca automáticamente el Rigidbody si no está asignado
        if (rb == null) rb = GetComponent<Rigidbody>();
        
        // Busca automáticamente al jugador por tag si no está asignado
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }
    }

    // =============================================
    // Actualización cada frame (lógica principal)
    // =============================================
    private void Update()
    {
        // Si no hay jugador, salir
        if (player == null) return;

        // Calcular distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Solo perseguir si el jugador está dentro del rango y fuera de distancia de parada
        if (distanceToPlayer <= detectionRange && distanceToPlayer > stoppingDistance)
        {
            MoveTowardsPlayer();
            RotateTowardsPlayer();
        }
    }

    // =============================================
    // Movimiento hacia el jugador (usando física)
    // =============================================
    private void MoveTowardsPlayer()
    {
        // Calcular dirección normalizada hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;
        
        // Aplicar velocidad al Rigidbody (movimiento basado en física)
        rb.linearVelocity = direction * moveSpeed;
    }

    // =============================================
    // Rotación suave hacia el jugador
    // =============================================
    private void RotateTowardsPlayer()
    {
        // Calcular dirección horizontal hacia el jugador (ignorando eje Y)
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        
        // Interpolación suave entre la rotación actual y la deseada
        transform.rotation = Quaternion.Slerp(
            transform.rotation,      // Rotación actual
            lookRotation,            // Rotación objetivo
            Time.deltaTime * rotationSpeed // Velocidad de rotación ajustada por tiempo
        );
    }

    // =============================================
    // Visualización de rangos en el Editor (sólo para debugging)
    // =============================================
    private void OnDrawGizmosSelected()
    {
        // Dibujar rango de detección (rojo)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Dibujar distancia de parada (verde)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}