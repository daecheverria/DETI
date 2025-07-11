using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private Health health;
    [SerializeField] private NivelesSO nivelesSO;
    public int numero;

    private void Start()
    {
        health = GetComponent<Health>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mario"))
        {
            // FX
            health.Damage(99, Vector3.zero);
            PlayerStatsManager.Instance.AddEstrellas(1);
            nivelesSO.SetNivelCompletado(numero, true);
        }

        // TODO: add sfx for dropping on ground
    }
}
