using UnityEngine;

public class Automata : MonoBehaviour
{
    [SerializeField] private int densidad; // Density of the noise

    private int[,] ruidoMatriz = new int[100, 100];
    [SerializeField] private int iteraciones; // Number of iterations for the cellular automaton
    public GameObject suelo;
    void Start()
    {
        Ruido(densidad);
        CelularAutomaton(iteraciones); // Number of iterations for the cellular automaton
        Instanciar(); // Instantiate the blocks based on the noise matrix

    }

    void Ruido(int densidad)
    {
        for (int i = 0; i < ruidoMatriz.GetLength(0); i++)
        {
            for (int j = 0; j < ruidoMatriz.GetLength(1); j++)
            {
                int random = Random.Range(0, 101); // Generar aleatorio por celda
                ruidoMatriz[i, j] = (random > densidad) ? 0 : 1;
            }
        }
    }
    void CelularAutomaton(int iterations)
    {
        for (int k = 0; k < iterations; k++)
        {

            int[,] tempMatriz = new int[ruidoMatriz.GetLength(0), ruidoMatriz.GetLength(1)];
            for (int i = 0; i < ruidoMatriz.GetLength(0); i++)
            {
                for (int j = 0; j < ruidoMatriz.GetLength(1); j++)
                {
                    int paredes = 0;
                    for (int x = i - 1; x <= i + 1; x++)
                    {
                        for (int y = j - 1; y <= j + 1; y++)
                        {
                            if (x >= 0 && x < ruidoMatriz.GetLength(0) && y >= 0 && y < ruidoMatriz.GetLength(1))
                            {
                                if (x == i && y == j) continue; // si es la misma celda, la saltamos
                                if (ruidoMatriz[x, y] == 1)
                                {
                                    paredes++;
                                }


                            }
                            else
                            {
                                paredes++;
                            }
                        }
                        
                    }
                    tempMatriz[i, j] = (paredes > 4) ? 1 : 0;
                }

            }
            ruidoMatriz = tempMatriz;
        }

    }
    void Instanciar()
    {
        for (int i = 0; i < ruidoMatriz.GetLength(0); i++)
        {
            for (int j = 0; j < ruidoMatriz.GetLength(1); j++)
            {
                GameObject bloque = Instantiate(suelo, new Vector3(i, 0, j), Quaternion.identity);
                bloque.GetComponent<Renderer>().material.color = (ruidoMatriz[i, j] == 1) ? Color.green : Color.blue;
            }
        }
    }
}
