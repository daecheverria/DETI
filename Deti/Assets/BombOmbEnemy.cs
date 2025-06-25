using UnityEngine;
using System.Collections;

public class BobOmbEnemy : MonoBehaviour
{
    [Header("Patrulla")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float waitTimeAtPoints = 1f;

    [Header("Persecución")]
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float explosionForce = 10f;   // Fuerza del empuje
    [SerializeField] private float explosionRadius = 5f;   // Radio del empuje

    private Transform target;
    private bool isChasing = false;
    private Rigidbody rb;

    private int currentPatrolIndex = 0;
    private bool isWaiting = false;

    // Referencia al Mario dentro del trigger (si hay)
    private Rigidbody marioRb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isChasing && target != null)
        {
            ChaseMario();
        }
        else
        {
            Patrol();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mario"))
        {
            // Guardar la referencia al Rigidbody de Mario
            marioRb = other.GetComponent<Rigidbody>();

            if (!isChasing)
            {
                target = other.transform;
                isChasing = true;
                Debug.Log("Bob-Omb empieza a perseguir a Mario");
                StartCoroutine(ChaseAndExplode());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mario"))
        {
            // Mario salió del rango
            marioRb = null;
        }
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0 || isWaiting) return;

        Transform point = patrolPoints[currentPatrolIndex];
        if (point == null) return;

        Vector3 targetPos = point.position;
        targetPos.y = transform.position.y;

        Vector3 direction = (targetPos - transform.position);
        direction.y = 0;
        direction.Normalize();

        rb.MovePosition(transform.position + direction * patrolSpeed * Time.fixedDeltaTime);

        if (direction != Vector3.zero)
            transform.forward = direction;

        float dist = Vector3.Distance(transform.position, targetPos);
        if (dist < 0.2f)
        {
            StartCoroutine(WaitAndNextPoint());
        }
    }

    private IEnumerator WaitAndNextPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTimeAtPoints);

        currentPatrolIndex++;
        if (currentPatrolIndex >= patrolPoints.Length)
            currentPatrolIndex = 0;

        isWaiting = false;
    }

    private void ChaseMario()
    {
        Vector3 direction = (target.position - transform.position);
        direction.y = 0;
        direction.Normalize();

        rb.MovePosition(transform.position + direction * chaseSpeed * Time.fixedDeltaTime);

        if (direction != Vector3.zero)
            transform.forward = direction;
    }

    private IEnumerator ChaseAndExplode()
    {
        yield return new WaitForSeconds(3f);

        Debug.Log("¡Bob-Omb explota!");

        // Si Mario está dentro, aplicamos empuje
        if (marioRb != null)
        {
            Vector3 explosionDir = (marioRb.position - transform.position).normalized;
            marioRb.AddForce(explosionDir * explosionForce, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }
}
