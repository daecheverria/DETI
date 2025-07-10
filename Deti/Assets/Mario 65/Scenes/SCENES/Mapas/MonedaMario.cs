using System;
using UnityEngine;

public class MonedaMario : MonoBehaviour
{
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
        // Si el GameObject tiene el tag "Rojo", cuenta la colisión y guarda en PlayerPrefs
        if (CompareTag("Rojo"))
        {
            ContarColisionRojo();
        }

        if (other.CompareTag("Mario"))
        {
            // Sumar punto al jugador usando PlayerPrefs
            int puntosActuales = PlayerPrefs.GetInt("Puntos", 0);
            PlayerPrefs.SetInt("Puntos", puntosActuales + 1);
            PlayerPrefs.Save();

            // Reproducir sonido de moneda usando MusicManager
            MusicManager.Instance.PlaySound("Moneda");

            // Destruir la moneda
            Destroy(gameObject);
        }
    }

    private void ContarColisionRojo()
    {
        int colisiones = PlayerPrefs.GetInt("ColisionesRojas", 0);
        PlayerPrefs.SetInt("ColisionesRojas", colisiones + 1);
        PlayerPrefs.Save();
    }
}
