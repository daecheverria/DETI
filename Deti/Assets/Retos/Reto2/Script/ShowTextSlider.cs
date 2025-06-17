using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ShowTextSlider : MonoBehaviour
{

    [SerializeField] private Slider barra;
    [SerializeField] private TextMeshProUGUI textoPorcentaje;
    
    public void ShowSliderValue()
    {
        if (textoPorcentaje != null && barra != null)
        {
            float valor = barra.value;
            textoPorcentaje.text = $"{valor}%";
        }
    }
}
