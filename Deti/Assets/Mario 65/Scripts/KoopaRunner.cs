using UnityEngine;
using System.Collections;

public class KoopaRunner : MonoBehaviour
{
    [Header("Ruta de carrera")]
    [SerializeField] private Transform[] racePoints;
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private float waitTimeAtPoints = 0.5f;

    private Rigidbody rb;
    private int currentPointIndex = 0;
    private bool isRunning = false;
    private bool isWaiting = false;
    private bool isFinish = false;

    [Header("Animación")]
    [SerializeField] private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetAnimationStates(false, false);
    }

    private void FixedUpdate()
    {
        if (isRunning && !isWaiting)
        {
            RunAlongPath();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mario") && !isRunning)
        {
            Debug.Log("¡Koopa empieza la carrera!");
            isRunning = true;
            isFinish = false;
            SetAnimationStates(isRunning, isFinish);
        }
    }

    private void RunAlongPath()
    {
        if (racePoints == null || currentPointIndex >= racePoints.Length) return;

        Transform targetPoint = racePoints[currentPointIndex];
        if (targetPoint == null) return;

        Vector3 targetPos = targetPoint.position;
        targetPos.y = transform.position.y;

        Vector3 direction = (targetPos - transform.position);
        direction.y = 0;
        direction.Normalize();

        rb.MovePosition(transform.position + direction * runSpeed * Time.fixedDeltaTime);

        if (direction != Vector3.zero)
            transform.forward = direction;

        float dist = Vector3.Distance(transform.position, targetPos);
        if (dist < 0.2f)
        {
            if (currentPointIndex == racePoints.Length - 1)
            {
                StartCoroutine(WaitAndStop());
            }
            else
            {
                currentPointIndex++; // Avanza al siguiente punto sin esperar
            }
        }
    }

    private IEnumerator WaitAndStop()
    {
        isWaiting = true;
        Debug.Log("Koopa llegó al final. Deteniéndose...");
        yield return new WaitForSeconds(waitTimeAtPoints);
        isRunning = false;
        isFinish = true;
        SetAnimationStates(isRunning, isFinish);
        isWaiting = false;
    }

    private void SetAnimationStates(bool running, bool finish)
    {
        if (animator != null)
        {
            animator.SetBool("isRunning", running);
            animator.SetBool("isFinish", finish);
        }
    }
}
