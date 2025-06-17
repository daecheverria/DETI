using UnityEngine;

[CreateAssetMenu(fileName = "MonedaScript", menuName = "Scriptable Objects/MonedaScript")]
public class MonedaScript : ScriptableObject
{
    [SerializeField] private int cantidadMonedas = 0;

    public int CantidadMonedas
    {
        get { return cantidadMonedas; }
        set { cantidadMonedas = value; }
    }
}
