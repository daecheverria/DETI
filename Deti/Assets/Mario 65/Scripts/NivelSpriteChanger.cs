using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NivelSpriteChanger : MonoBehaviour
{
    [SerializeField] private NivelesSO nivelSO;
    [SerializeField] private int indiceNivel;
    [SerializeField] private Sprite completadoSprite;
    [SerializeField] private Sprite noCompletadoSprite;
    [SerializeField] private Image image;

    void Awake()
    {
        if (image == null)
            image = GetComponentInChildren<Image>();
        ActualizarSprite();
    }

    public void ActualizarSprite()
    {
        if (image == null) return;
        if (nivelSO.GetNivelCompletado(indiceNivel))
            image.sprite = completadoSprite;
        else
            image.sprite = noCompletadoSprite;
    }
    public void OnClick()
    {
        SceneManager.LoadScene(3);
    }
}