using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject nivel1;
    [SerializeField] private GameObject nivel2;
    [SerializeField] private List<GameObject> MonedasRojas; 
    [SerializeField] private GameObject estrella;

    void Start()
    {
        if(PlayerStatsManager.Instance.level == 0)
        {
            nivel1.SetActive(true);
        }else if(PlayerStatsManager.Instance.level == 1)
        {
            nivel2.SetActive(true);
        }
    }

    void Update()
    {
        // Elimina referencias nulas (objetos destruidos)
        MonedasRojas.RemoveAll(obj => obj == null);

        // Si la lista está vacía, haz algo
        if (MonedasRojas.Count == 0)
        {
            TodosObjetosDestruidos();
        }
    }

    private void TodosObjetosDestruidos()
    {
        estrella.SetActive(true);
    }
}
