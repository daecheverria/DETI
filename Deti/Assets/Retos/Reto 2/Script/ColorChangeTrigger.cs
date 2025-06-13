using UnityEngine;
public class ColorChangeTrigger : MonoBehaviour
{
    // =============================================
    // Configuración de colores (visible en el Inspector)
    // =============================================
    [Header("Color Settings")]
    [SerializeField] private Color enterColor = Color.red;
    // Color que se aplicará cuando el jugador entre al trigger
    [SerializeField] private Color exitColor = Color.white;
    // Color que se aplicará cuando el jugador salga del trigger
    [SerializeField] private float colorChangeSpeed = 5f;
    // Velocidad de la transición entre colores (en unidades por segundo)

    // =============================================
    // Referencia al objeto que cambiará de color
    // =============================================
    [Header("Target Object")]
    [SerializeField] private Renderer targetRenderer;
    // Componente Renderer del objeto que cambiará de color

    // =============================================
    // Variables privadas de estado
    // =============================================
    private Color originalColor;
    // Guarda el color inicial del objeto
    private bool playerInside = false;
    // Bandera que indica si el jugador está dentro del trigger

    // =============================================
    // Inicialización (se ejecuta al inicio)
    // =============================================
    private void Start()
    {
        // Si no se asignó un renderer en el Inspector, intentamos obtener el del mismo objeto
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }
        // Si encontramos un renderer, guardamos su color original
        if (targetRenderer != null)
        {
            originalColor = targetRenderer.material.color;
        }
        else
        {
            // Mensaje de advertencia si no hay renderer
            Debug.LogWarning("No se encontró Renderer en el objeto: " + gameObject.name);
        }
    }

    // =============================================
    // Actualización cada frame (lógica de cambio de color)
    // =============================================
    private void Update()
    {
        // Si no hay renderer, salimos
        if (targetRenderer == null) return;
        // Determinamos el color objetivo según si el jugador está dentro o fuera
        Color targetColor = playerInside ? enterColor : exitColor;
        // Interpolación suave entre el color actual y el objetivo
        targetRenderer.material.color = Color.Lerp(
            targetRenderer.material.color, // Color actual
            targetColor, // Color objetivo
            colorChangeSpeed * Time.deltaTime 
            // Velocidad de transición (ajustada por tiempo de frame)
        );
    }

    // =============================================
    // Evento: Cuando otro collider entra al trigger
    // =============================================
    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el objeto que entró es el jugador (usando tag)
        if (other.CompareTag("Player"))
        {
            playerInside = true; // Actualizamos el estado
        }
    }

    // =============================================
    // Evento: Cuando otro collider sale del trigger
    // =============================================
    private void OnTriggerExit(Collider other)
    {
        // Verificamos si el objeto que salió es el jugador
        if (other.CompareTag("Player"))
        {
            playerInside = false; // Actualizamos el estado
        }
    }

    // =============================================
    // Resetear color al desactivar el objeto
    // =============================================
    private void OnDisable()
    {
        // Si hay renderer, restauramos su color original
        if (targetRenderer != null)
        {
            targetRenderer.material.color = originalColor;
        }
    }
}