using UnityEngine;
using UnityEngine.SceneManagement;

public class Backroom : MonoBehaviour
{
    [Header("Nombre de la escena a cargar")]
    [SerializeField] private string nombreEscena = "NombreDeLaEscena";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mario"))
        {
            if (!string.IsNullOrEmpty(nombreEscena))
            {
                SceneManager.LoadScene(nombreEscena);
            }
            else
            {
                Debug.LogError("Backroom: No se ha asignado el nombre de la escena a cargar.");
            }
        }
    }
}
