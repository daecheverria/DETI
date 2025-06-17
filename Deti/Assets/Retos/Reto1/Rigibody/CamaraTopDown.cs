using UnityEngine;

public class CamaraTopDown : MonoBehaviour
{
    [SerializeField] private Transform jugador;
    [SerializeField] private Vector3 offset = new Vector3(0f, 10f, -3f);
    [SerializeField] private float suavizado = 5f;

    void LateUpdate()
    {
        Vector3 posicionDeseada = jugador.position + offset;
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
        transform.LookAt(jugador);
    }
}