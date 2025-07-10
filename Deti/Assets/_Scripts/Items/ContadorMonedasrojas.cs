using System.Collections.Generic;
using UnityEngine;

public class ContadorMonedasrojas : MonoBehaviour
{
    [Header("Objetos a vigilar (7 en total)")]
    [SerializeField] private List<GameObject> objetosAVigilar;

    [Header("Objeto a activar")]
    [SerializeField] private GameObject objetoActivar;

    private void Update()
    {
        // Elimina referencias nulas (objetos destruidos) de la lista
        objetosAVigilar.RemoveAll(obj => obj == null);

        // Si todos los objetos han sido eliminados y el objeto a activar está inactivo
        if (objetosAVigilar.Count == 0 && objetoActivar != null && !objetoActivar.activeSelf)
        {
            objetoActivar.SetActive(true);
        }
    }
}
