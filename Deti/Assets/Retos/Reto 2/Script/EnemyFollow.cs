using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float wanderRadius = 5f; // Radio de deambulación
    [SerializeField] private float wanderTimer = 5f; // Tiempo entre cambios de dirección

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody rb;

    [Header("Orientation Settings")]
    [SerializeField] private float forwardAngleOffset = 90f;

    [Header("Player Damage")]
    [SerializeField] private float damageCooldown = 1f;
    private float lastDamageTime;

    [Header("Bounce Settings")]
    [SerializeField] private float bounceForce = 100f;

    [Header("Enemy State")]
    private bool isDying = false;
    private Collider enemyCollider;
    private Vector3 wanderPoint; // Punto actual de deambulación
    private float timer; // Temporizador para cambio de dirección

    private void Awake()
    {
        enemyCollider = GetComponent<Collider>();
        if (rb == null) rb = GetComponent<Rigidbody>();

        if (player == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Mario");
            if (playerObj != null) player = playerObj.transform;
        }

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ |
                         RigidbodyConstraints.FreezePositionY;

        // Establecer primer punto de deambulación
        wanderPoint = GetRandomWanderPoint();
    }

    private void Update()
    {
        if (isDying) return;

        if (player != null && IsPlayerDetected())
        {
            ChasePlayer();
        }
        else
        {
            WanderAround();
        }
    }

        private bool IsPlayerDetected()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer <= detectionRange;
    }

    private void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            MoveTowardsPlayer();
            RotateTowardsPlayer();
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void WanderAround()
    {
        timer += Time.deltaTime;

        // Cambiar de dirección cuando se cumple el tiempo
        if (timer >= wanderTimer)
        {
            wanderPoint = GetRandomWanderPoint();
            timer = 0;
        }

        // Mover hacia el punto de deambulación
        Vector3 direction = (wanderPoint - transform.position).normalized;
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            rb.linearVelocity = direction * moveSpeed * 0.5f; // Más lento al deambular

            // Rotación hacia la dirección de movimiento
            Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, forwardAngleOffset, 0);
            float targetYRotation = targetRotation.eulerAngles.y;
            Quaternion flatTargetRotation = Quaternion.Euler(0, targetYRotation, 0);
            
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                flatTargetRotation,
                Time.deltaTime * rotationSpeed * 0.5f // Rotación más lenta
            );
        }

        // Si llegó cerca del punto, buscar uno nuevo
        if (Vector3.Distance(transform.position, wanderPoint) < 0.5f)
        {
            wanderPoint = GetRandomWanderPoint();
        }
    }

    private Vector3 GetRandomWanderPoint()
    {
        // Obtener un punto aleatorio dentro del radio de deambulación
        Vector3 randomPoint = Random.insideUnitSphere * wanderRadius;
        randomPoint += transform.position;
        randomPoint.y = transform.position.y; // Mantener misma altura

        // Asegurarse que el punto es accesible (opcional)
        if (Physics.Raycast(randomPoint, -Vector3.up, 2f))
        {
            return randomPoint;
        }
        return transform.position; // Si no es accesible, quedarse donde está
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isDying) return;
        if (other.CompareTag("Mario"))
        {
            isDying = true;

            // Rebote del jugador
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0, playerRb.linearVelocity.z); // Reinicia Y
                playerRb.AddForce(Vector3.up * bounceForce, ForceMode.VelocityChange);
            }
            Destroy(gameObject, 0.02f);
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Eliminar componente vertical

        // Aplicar velocidad manteniendo el offset de orientación
        rb.linearVelocity = direction * moveSpeed;
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Solo rotación horizontal

        if (direction.magnitude > 0.1f)
        {
            // Calcular rotación con el offset para que el costado sea el frente
            Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, forwardAngleOffset, 0);

            // Extraer solo la rotación en Y
            float targetYRotation = targetRotation.eulerAngles.y;
            Quaternion flatTargetRotation = Quaternion.Euler(0, targetYRotation, 0);

            // Rotación suave solo en Y
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                flatTargetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDying) return;

        if (collision.gameObject.CompareTag("Mario"))
        {
            TryDamagePlayer();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isDying) return;

        if (collision.gameObject.CompareTag("Mario"))
        {
            TryDamagePlayer();
        }
    }

    private void TryDamagePlayer()
    {
        if (Time.time - lastDamageTime >= damageCooldown)
        {
            if (PlayerStatsManager.Instance != null)
            {
                PlayerStatsManager.Instance.AddVidas(-1);
                lastDamageTime = Time.time;
            }
            else
            {
                Debug.LogWarning("PlayerStatsManager.Instance no encontrado!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);

        // Dibuja la dirección "frontal" (costado) con el offset aplicado
        Gizmos.color = Color.blue;
        Vector3 forwardDirection = Quaternion.Euler(0, forwardAngleOffset, 0) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + forwardDirection * 2);
    }
    
}