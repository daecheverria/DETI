using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();
        this.gameObject.SetActive(false); // Desactiva el objeto al inicio
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mario"))
        {
            // FX
            health.Damage(99, Vector3.zero);
            PlayerStatsManager.Instance.AddEstrellas(1);
        }

        // TODO: add sfx for dropping on ground
    }
}
