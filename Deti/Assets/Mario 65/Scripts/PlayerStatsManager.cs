using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerStatsManager : MonoBehaviour
{
    // Singleton para acceso global a la instancia de PlayerStatsManager
    public static PlayerStatsManager Instance { get; private set; }

    // Estadísticas del jugador (valores entre 0 y 100)
    public int _vidas = 3; // Número de vidas del jugador
    public int _estrellas = 0;
    public int _monedas = 0;
    public delegate void StatChanged(int valor);
    public static event StatChanged OnVidasChanged;
    public static event StatChanged OnEstrellasChanged;
    public static event StatChanged OnMonedasChanged;
    public int level = 0;
    [SerializeField] private GameObject estrella;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        estrella = GameObject.FindWithTag("Estrella");
        estrella.SetActive(false);
        _monedas = 0;
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (Instance != null && Instance != this)
        {
            Debug.Log($"¡Múltiples instancias de PlayerStatsManager! Destruyendo {gameObject.name}");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Mantiene el objeto al cambiar de escena
        Debug.Log($"PlayerStatsManager inicializado. Instancia ID: {GetInstanceID()}");
        OnVidasChanged?.Invoke(_vidas); // Notifica el cambio de vidas
        OnEstrellasChanged?.Invoke(_estrellas); // Notifica el cambio de estrellas
        OnMonedasChanged?.Invoke(_monedas); // Notifica el cambio de monedas
    }

    /// <summary>
    /// Incrementa el número de vidas del jugador y notifica el cambio.
    /// </summary>
    public void AddVidas(int cantidad)
    {
        _vidas += cantidad;
        OnVidasChanged?.Invoke(_vidas);
    }

    /// <summary>
    /// Incrementa el número de estrellas del jugador y notifica el cambio.
    /// </summary>
    public void AddEstrellas(int cantidad)
    {
        _estrellas += cantidad;
        OnEstrellasChanged?.Invoke(_estrellas);
    }

    /// <summary>
    /// Incrementa el número de monedas del jugador y notifica el cambio.
    /// </summary>
    public void AddMonedas(int cantidad)
    {
        _monedas += cantidad;
        OnMonedasChanged?.Invoke(_monedas);

        if (_monedas >= 100)
        {
            if (estrella != null)
            {
                estrella.SetActive(true);
            }
        }
        if (_vidas < 6)
        {
            AddVidas(1);
        }
    }

    public int Vidas => _vidas;
    public int Estrellas => _estrellas;
    public int Monedas => _monedas;

    public void cambiarNivel(int nuevoNivel)
    {
        level = nuevoNivel;
    }
}
    
