using System;
using UnityEngine;

public class MonedaMario : MonoBehaviour
{
    [Header("Sonido de recogida")]
    [SerializeField] private AudioClip sonidoMoneda;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void AddForceOnSpawn(Vector3 force)
    {
        if (rb != null)
        {
            rb.AddForce(force, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mario"))
        {
            // Sumar punto al jugador usando PlayerPrefs
            int puntosActuales = PlayerPrefs.GetInt("Puntos", 0);
            PlayerPrefs.SetInt("Puntos", puntosActuales + 1);
            PlayerPrefs.Save();

            // Reproducir sonido en la posición de la moneda
            if (sonidoMoneda != null)
                AudioSource.PlayClipAtPoint(sonidoMoneda, transform.position);

            // Destruir la moneda
            Destroy(gameObject);
        }
    }
}
