using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float force = 5f;
    [SerializeField] private bool damagePlayer = false;
    [SerializeField] private bool explode = false;

    private void OnTriggerEnter(Collider other)
    {
        // Daño al jugador
        if (damagePlayer && other.CompareTag("Player"))
        {
            Vector3 knockBack = other.transform.position - transform.position;
            knockBack = knockBack.normalized * force;

            other.GetComponent<Health>()?.Damage(damage, knockBack);

            if (explode)
            {
                Health myHealth = GetComponentInParent<Health>(); // Aquí!
                if (myHealth != null)
                    myHealth.Damage(999, Vector3.zero);
            }
        }
        // Daño a enemigo o caja
        else if (other.CompareTag("Enemy") || other.CompareTag("Crate"))
        {
            Vector3 knockBack = other.transform.position - transform.position;
            knockBack = knockBack.normalized * force;

            other.GetComponent<Health>()?.Damage(damage, knockBack);
        }
    }
}
