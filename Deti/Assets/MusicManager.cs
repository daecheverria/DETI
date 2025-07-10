using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string nombre;
        public AudioClip clip;
    }

    [Header("Lista de sonidos")]
    [SerializeField] private List<Sound> sonidos = new List<Sound>();

    private AudioSource audioSource;
    private string sonidoEnLoopActual = null;

    // Singleton para acceso global
    public static MusicManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// Reproduce un sonido por su nombre (OneShot).
    /// </summary>
    public void PlaySound(string nombre)
    {
        Sound sonido = sonidos.Find(s => s.nombre == nombre);
        if (sonido != null && sonido.clip != null)
        {
            audioSource.PlayOneShot(sonido.clip);
        }
        else
        {
            Debug.LogWarning($"MusicManager: Sonido '{nombre}' no encontrado.");
        }
    }

    /// <summary>
    /// Reproduce un sonido en loop (por ejemplo, caminar/correr).
    /// </summary>
    public void PlayLoop(string nombre)
    {
        if (sonidoEnLoopActual == nombre && audioSource.isPlaying && audioSource.loop)
            return; // Ya está sonando ese sonido en loop

        Sound sonido = sonidos.Find(s => s.nombre == nombre);
        if (sonido != null && sonido.clip != null)
        {
            audioSource.clip = sonido.clip;
            audioSource.loop = true;
            audioSource.Play();
            sonidoEnLoopActual = nombre;
        }
        else
        {
            Debug.LogWarning($"MusicManager: Sonido loop '{nombre}' no encontrado.");
        }
    }

    /// <summary>
    /// Detiene el sonido en loop si está sonando.
    /// </summary>
    public void StopLoop()
    {
        if (audioSource.loop)
        {
            audioSource.Stop();
            audioSource.loop = false;
            sonidoEnLoopActual = null;
        }
    }
}
