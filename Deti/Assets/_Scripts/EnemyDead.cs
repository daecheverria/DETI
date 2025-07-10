using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    public GameObject enemy;
    public GameObject star;
    public float bounceForce = 25f; // Fuerza del rebote

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mario"))
        {
            // Rebote del jugador
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0, playerRb.linearVelocity.z); // Reinicia Y
                playerRb.AddForce(Vector3.up * bounceForce, ForceMode.VelocityChange);
            }
            Destroy(enemy);
            star.SetActive(true); // Activa la estrella
        }
    }
}
