using UnityEngine;

public class AlgoritmoCelular : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private int ancho = 160;
    [SerializeField] private int alto = 90;
    [SerializeField][Range(0, 100)] private int densidadRuido = 50;
    [SerializeField] private int iteraciones = 5;

    [Header("Prefabs")]
    public GameObject suelo;
    public GameObject pared;

    private int[,] grid;

    private void Awake()
    {
        InicializarGrid();
        GenerarMapaProcedural();
    }

    private void InicializarGrid()
    {
        grid = new int[ancho, alto];
    }

    public void GenerarMapaProcedural()
    {
        GenerarRuido();
        for (int i = 0; i < iteraciones; i++)
        {
            AplicarReglasAutomatas();
        }
        InstanciarMapa();
    }

    private void GenerarRuido()
    {
        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < alto; y++)
            {
                grid[x, y] = (Random.Range(0, 100) > densidadRuido) ? 1 : 0;
            }
        }
    }

    private void AplicarReglasAutomatas()
    {
        int[,] nuevoGrid = (int[,])grid.Clone();

        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < alto; y++)
            {
                int vecinosPared = ContarVecinosPared(x, y);
                bool esBorde = (x == 0 || y == 0 || x == ancho - 1 || y == alto - 1);

                nuevoGrid[x, y] = (vecinosPared > 4 || esBorde) ? 0 : 1;
            }
        }

        grid = nuevoGrid;
    }

    private int ContarVecinosPared(int x, int y)
    {
        int contador = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                int vecinoX = x + i;
                int vecinoY = y + j;

                if (vecinoX >= 0 && vecinoX < ancho && vecinoY >= 0 && vecinoY < alto)
                {
                    if (grid[vecinoX, vecinoY] == 0) contador++;
                }
            }
        }
        return contador;
    }

    private void InstanciarMapa()
    {
        foreach (Transform hijo in transform)
        {
            Destroy(hijo.gameObject);
        }

        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < alto; y++)
            {
                Vector3 posicion = new Vector3(x, y, 0);
                GameObject prefab = (grid[x, y] == 1) ? suelo : pared;
                Instantiate(prefab, posicion, Quaternion.identity, transform);
            }
        }
    }

    public void EstablecerDensidadRuido(float densidad) => densidadRuido = Mathf.RoundToInt(densidad);
    public void EstablecerIteraciones(int iteraciones) => this.iteraciones = iteraciones;
    public void RegenerarMapa() => GenerarMapaProcedural();
}