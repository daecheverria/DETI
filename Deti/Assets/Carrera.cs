using UnityEngine;

public class Carrera : MonoBehaviour
{
    [SerializeField] private GameObject estrella; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mario"))
        {
            if (estrella != null)
                estrella.SetActive(true);
        }
    }
}
