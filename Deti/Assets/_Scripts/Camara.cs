using UnityEngine;

public class Camara : MonoBehaviour
{
    public Transform target; // El personaje a seguir
    public float distance = 6f; // Distancia de la cámara al personaje
    public float height = 2f;   // Altura de la cámara respecto al personaje
    public float sensitivity = 3f; // Sensibilidad del mouse
    public float smoothSpeed = 8f; // Suavidad del seguimiento

    private float yaw = 0f;
    private float pitch = 15f;
    public float minPitch = -10f;
    public float maxPitch = 60f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Rotación con el mouse
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Calcular la posición deseada
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        Vector3 desiredPosition = target.position + offset + Vector3.up * height;

        // Suavizar el movimiento de la cámara
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Mirar al personaje
        transform.LookAt(target.position + Vector3.up * height);
    }
}
