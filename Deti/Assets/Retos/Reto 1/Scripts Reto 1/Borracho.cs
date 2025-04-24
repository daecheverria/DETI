using Math = System.Math;
using UnityEngine;

public class Borracho : MonoBehaviour
{
    private int[,] mapMatrix = new int[100, 100];
    public GameObject suelo;
    void Start()
    {
        AlgoritmoBorracho();
        Instanciar();
        //PrintMapMatrix();
    }

    void AlgoritmoBorracho()
    {
        int x = Random.Range(0, mapMatrix.GetLength(0));
        int y = Random.Range(0, mapMatrix.GetLength(1)); 
        int steps = (int)Math.Round(mapMatrix.Length*0.6); // Number of steps to take
        int stepsfilled = 0; // Number of steps filled
        while(stepsfilled < steps)
        {
            // Check if the current position is already filled
            if (mapMatrix[x, y] == 0)
            {
                mapMatrix[x, y] = 1; // Mark the cell as visited
                stepsfilled++;
            }

            // Randomly choose a direction to move
            int direction = Random.Range(0, 4); // 0: up, 1: down, 2: left, 3: right

            switch (direction)
            {
                case 0: // Up
                    if (x > 0) x--;
                    break;
                case 1: // Down
                    if (x < mapMatrix.GetLength(0) - 1) x++;
                    break;
                case 2: // Left
                    if (y > 0) y--;
                    break;
                case 3: // Right
                    if (y < mapMatrix.GetLength(1) - 1) y++;
                    break;
            }
        }
        // for (int i = 0; i < steps; i++)
        // {
        //     // Randomly choose a direction to move
        //     int direction = Random.Range(0, 4); // 0: up, 1: down, 2: left, 3: right

        //     switch (direction)
        //     {
        //         case 0: // Up
        //             if (x > 0) x--;
        //             break;
        //         case 1: // Down
        //             if (x < mapMatrix.GetLength(0) - 1) x++;
        //             break;
        //         case 2: // Left
        //             if (y > 0) y--;
        //             break;
        //         case 3: // Right
        //             if (y < mapMatrix.GetLength(1) - 1) y++;
        //             break;
        //     }

        //     if (mapMatrix[x, y] == 0)
        //     {
        //         mapMatrix[x, y] = 1; // Mark the cell as visited
        //     }
        // }
    }

    void Instanciar(){
        for (int i = 0; i < mapMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < mapMatrix.GetLength(1); j++)
            {
                    GameObject bloque = Instantiate(suelo, new Vector3(i, 0, j), Quaternion.identity);
                    bloque.GetComponent<Renderer>().material.color = (mapMatrix[i, j] == 1) ? Color.green : Color.blue;
            }
        }
    }
    void PrintMapMatrix()
    {
        string mapString = "";
        for (int i = 0; i < mapMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < mapMatrix.GetLength(1); j++)
            {
                mapString += mapMatrix[i, j] + " ";
            }
            mapString += "\n";
        }
        Debug.Log(mapString);
    }
}