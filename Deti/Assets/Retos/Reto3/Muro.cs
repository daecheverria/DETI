using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; // Required for UI components
using TMPro; // Required for TextMeshPro components

public class Muro : MonoBehaviour
{
    public MonedaScript monedaScript; // Reference to the MonedaScript component
    public TextMeshProUGUI textoMonedas; // Reference to the TextMeshProUGUI component
    public GameObject muro; // Reference to the GameObject that represents the wall
    public int monedasRequeridas = 8; // Number of coins required to pass through the wall


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MonedaScript moneda = monedaScript; // Get the current MonedaScript instance

    }

    private IEnumerator WaitForSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        textoMonedas.text = ""; // Limpia el texto de las monedas
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (monedaScript.CantidadMonedas >= monedasRequeridas)
            {
                // Si el jugador tiene suficientes monedas, destruir el muro
                muro.SetActive(false); // Desactiva el muro
                textoMonedas.text = "¡Gracias por pagar el peaje!"; // Actualiza el texto de las monedas
                monedaScript.CantidadMonedas -= monedasRequeridas; // Resta las monedas requeridas
                StartCoroutine(WaitForSeconds(2f)); // Inicia la corrutina para limpiar el texto después de 2 segundos


            }

            else
            {
                textoMonedas.text = "¡Necesitas " + monedasRequeridas + " monedas para pasar!"; // Actualiza el texto de las monedas
                StartCoroutine(WaitForSeconds(2f)); // Inicia la corrutina para limpiar el texto después de 2 segundos

            }
        }

       
    }

    
}
