using UnityEngine;
using TMPro;
using System.Collections;

public class HUDController : MonoBehaviour
{
    public TMP_Text Vidas;
    public TMP_Text Estrellas;
    public TMP_Text Monedas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Inicializa los textos del HUD con las estadísticas del jugador
        if (PlayerStatsManager.Instance != null)
        {
            Vidas.text = PlayerStatsManager.Instance.Vidas.ToString();
            Estrellas.text = PlayerStatsManager.Instance.Estrellas.ToString();
            Monedas.text = PlayerStatsManager.Instance.Monedas.ToString();
        }
        else
        {
            Debug.LogWarning("PlayerStatsManager no encontrado. Asegúrate de que esté inicializado antes de HUDController.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStatsManager.Instance != null)
        {
            Vidas.text = PlayerStatsManager.Instance.Vidas.ToString();
            Estrellas.text = PlayerStatsManager.Instance.Estrellas.ToString();
            Monedas.text = PlayerStatsManager.Instance.Monedas.ToString();
        }
        else
        {
            Debug.LogWarning("PlayerStatsManager no encontrado. Asegúrate de que esté inicializado antes de HUDController.");
        }
    }
}
