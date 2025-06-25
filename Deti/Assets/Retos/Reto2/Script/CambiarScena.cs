using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CambiarScena : MonoBehaviour
{
    public GameObject panelCarga; // Asigna el panel desde el inspector

    public void CambiarScenaBoton(int numeroEscena)
    {
        StartCoroutine(FadePanel(numeroEscena));
    }

    public static CambiarScena instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Cambia al número de escena que desees, por ejemplo 1
            CambiarScenaBoton(2);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Cambia al número de escena que desees, por ejemplo 1
            CambiarScenaBoton(3);
        }
    }

    public void SalirJuego()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    IEnumerator FadePanel(int numeroEscena)
    {
        panelCarga.SetActive(true); // Muestra el panel de carga

        // Espera un pequeño tiempo para mostrar el panel (opcional)
        yield return new WaitForSeconds(0.5f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(numeroEscena);
        yield return new WaitUntil(() => asyncLoad.isDone);

        // Aquí puedes ocultar el panel si la escena no destruye este objeto
        panelCarga.SetActive(false);
    }
}
