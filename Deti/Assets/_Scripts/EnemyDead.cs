using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    public float bounceForce = 100f; // Fuerza del rebote

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Rebote del jugador
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0, playerRb.linearVelocity.z); // Reinicia Y
                playerRb.AddForce(Vector3.up * bounceForce, ForceMode.VelocityChange);
            }
            Destroy(gameObject);
        }
    }
}
