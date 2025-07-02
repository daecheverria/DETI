using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NivelesSO", menuName = "Scriptable Objects/NivelesSO")]
public class NivelesSO : ScriptableObject
{
    [SerializeField]
    private List<bool> nivelesCompletados = new List<bool>();

    public bool GetNivelCompletado(int indice)
    {
        if (indice < 0 || indice >= nivelesCompletados.Count)
            return false;
        return nivelesCompletados[indice];
    }

    public void SetNivelCompletado(int indice, bool completado)
    {
        if (indice < 0)
            return;
        nivelesCompletados[indice] = completado;
    }
}
