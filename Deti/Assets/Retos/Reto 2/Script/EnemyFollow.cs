using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float stoppingDistance = 1f;
    [SerializeField] private float detectionRange = 10f;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody rb;

    [Header("Orientation Settings")]
    [SerializeField] private float forwardAngleOffset = 90f; // 90° para que el costado sea el frente

    [Header("Player Damage")]
    [SerializeField] private float damageCooldown = 1f; // Tiempo entre daños
    private float lastDamageTime; // Cuando fue el último daño

    [Header("Bounce Settings")]
    [SerializeField] private float bounceForce = 100f; // Fuerza del rebote

    [Header("Enemy State")]
    private bool isDying = false;
    private Collider enemyCollider;

    private void Awake()
    {
        enemyCollider = GetComponent<Collider>();
        if (rb == null) rb = GetComponent<Rigidbody>();

        if (player == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Mario");
            if (playerObj != null) player = playerObj.transform;
        }

        // Congelar rotaciones no deseadas y movimiento en Y
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ |
                         RigidbodyConstraints.FreezePositionY;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && distanceToPlayer > stoppingDistance)
        {
            MoveTowardsPlayer();
            RotateTowardsPlayer();
        }
        else
        {
            rb.linearVelocity = new Vector3(0, 0, 0); // Detener completamente
        }
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