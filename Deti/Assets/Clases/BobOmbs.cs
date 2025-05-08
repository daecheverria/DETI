using UnityEngine;

public class BobOmbs : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float fuseTime = 5f;
    public float explosionRadius = 3f;
    public LayerMask targetLayer; // Capa de los objetos que serán afectados por la explosión
    public GameObject explosionEffectPrefab; // Prefab de la explosión (opcional)

    private Rigidbody rb;
    private Transform playerHolding = null;
    private float timer;
    private bool isHeld = false;
    private bool isTriggered = false;
    private Renderer rend;
    private float colorChangeTimer = 0f;
    private Color color1 = Color.black; // Color inicial
    private Color color2 = Color.red; // Color alternativo
    public Transform mario; // Referencia al transform de Mario
    public float activationDistance = 5f; // Distancia a la que Bob-omb se activa al acercarse a Mario

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timer = fuseTime;
        rend = GetComponent<Renderer>();
        rend.material.color = color1; // Establecer el color inicial
    }

    void Update()
    {
        if (!isHeld)
        {
            // Movimiento
            rb.linearVelocity = transform.forward * moveSpeed;
            float randomRotation = Random.Range(-50f, 50f) * Time.deltaTime;
            rb.angularVelocity = new Vector3(0, randomRotation, 0);

            // Comprobar distancia a Mario
            if (!isTriggered && mario != null)
            {
                float distance = Vector3.Distance(transform.position, mario.position);
                if (distance <= activationDistance)
                {
                    isTriggered = true;
                }
            }

            // Si fue activado, contar y parpadear
            if (isTriggered)
            {
                timer -= Time.deltaTime;

                // Alternar colores cada 0.2s
                colorChangeTimer += Time.deltaTime;
                if (colorChangeTimer >= 0.2f)
                {
                    rend.material.color = rend.material.color == color1 ? color2 : color1;
                    colorChangeTimer = 0f;


                }

                if (timer <= 0)
                {
                    Explode();
                }
            }
        }
        else
        {
            // Si está siendo sostenido, sigue la posición del jugador
            if (playerHolding != null)
            {
                transform.position = playerHolding.position;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    // Función para ser recogido
    public void PickUp(Transform player)
    {
        isHeld = true;
        playerHolding = player;
        rb.isKinematic = true; // Para que no le afecten las físicas mientras lo sostiene
    }

    // Función para ser lanzado
    public void Throw(Vector3 throwForce)
    {
        isHeld = false;
        playerHolding = null;
        rb.isKinematic = false;
        rb.linearVelocity = throwForce;
        timer = fuseTime; // Reiniciar el temporizador al ser lanzado
    }

    // Función de explosión
    void Explode()
    {
        // Mostrar efecto de explosión si hay un prefab asignado
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Detectar objetos en el radio de la explosión
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, targetLayer);

        // Aplicar lógica a los objetos afectados (puedes modific esto según tus necesidades)
        foreach (Collider hitCollider in hitColliders)
        {
            // Ejemplo: Si el objeto tiene un componente "Destructible", llamamos a su función "TakeDamage"
            // Destructible destructible = hitCollider.GetComponent<Destructible>();
            // if (destructible != null)
            // {
            //     destructible.TakeDamage();
            // }

            // Ejemplo: Aplicar una fuerza de empuje (si el objeto tiene un Rigidbody)
            Rigidbody targetRb = hitCollider.GetComponent<Rigidbody>();
            if (targetRb != null)
            {
                targetRb.AddExplosionForce(500f, transform.position, explosionRadius, 1f, ForceMode.Impulse);
            }
        }

        // Destruir el Bob-omb después de explotar
        Destroy(gameObject);
    }

    // Dibujar un gizmo en el editor para visualizar el radio de la explosión
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
