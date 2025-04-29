using Unity.Collections;
using UnityEngine;
using Math = System.Math;
public class DiamanteCuadrado : MonoBehaviour
{
    public int[,] matriz;
    int maxHeight = 16; 
    int minHeight = 1; 
    [SerializeField] int randomStart = 8;
    float random;
    public GameObject suelo;
    void Start()
    {
        random = randomStart; 
        matriz = diamante_cuadrado(); 
        Instanciar();
    }
    float randomDC(){
        return Random.Range(-1,2)*random;
    }
    int[,] diamante_cuadrado()
    {
        int heightMapSize = 129;
        int[,] heightMap = new int[heightMapSize, heightMapSize];

        heightMap[0, 0] = Random.Range(minHeight, maxHeight);
        heightMap[0, heightMapSize - 1] = Random.Range(minHeight, maxHeight);
        heightMap[heightMapSize - 1, 0] = Random.Range(minHeight, maxHeight);
        heightMap[heightMapSize - 1, heightMapSize - 1] = Random.Range(minHeight, maxHeight);

        int chunkSize = heightMapSize - 1;

        while (chunkSize > 1)
        {
            int half = chunkSize / 2;

            // Paso cuadrado
            for (int y = 0; y < heightMapSize - 1; y += chunkSize)
            {
                for (int x = 0; x < heightMapSize - 1; x += chunkSize)
                {
                    float avg = (
                        heightMap[y, x] +
                        heightMap[y, x + chunkSize] +
                        heightMap[y + chunkSize, x] +
                        heightMap[y + chunkSize, x + chunkSize]
                    ) / 4f;

                    heightMap[y + half, x + half] = Mathf.Clamp(Mathf.RoundToInt(avg + randomDC()),minHeight, maxHeight);
                }
            }

            // Paso diamante
            for (int y = 0; y < heightMapSize; y += half)
            {
                for (int x = (y + half) % chunkSize; x < heightMapSize; x += chunkSize)
                {
                    float sum = 0f;
                    int count = 0;

                    if (x - half >= 0)
                    {
                        sum += heightMap[y, x - half];
                        count++;
                    }
                    if (x + half < heightMapSize)
                    {
                        sum += heightMap[y, x + half];
                        count++;
                    }
                    if (y - half >= 0)
                    {
                        sum += heightMap[y - half, x];
                        count++;
                    }
                    if (y + half < heightMapSize)
                    {
                        sum += heightMap[y + half, x];
                        count++;
                    }

                    heightMap[y, x] = Mathf.Clamp(
                        Mathf.RoundToInt(sum / count + randomDC()),
                        minHeight, maxHeight);
                }
            }

            chunkSize /= 2;
            random = Mathf.Max(random / 2f, 0.1f);
        }

        return heightMap;
    }

    void Instanciar()
    {
        for (int i = 0; i < matriz.GetLength(0); i++)
        {
            for (int j = 0; j < matriz.GetLength(1); j++)
            {
                GameObject bloque = Instantiate(suelo, new Vector3(i, 0, j), Quaternion.identity);
                Color colorBloque;

                float variation = Random.Range(-0.05f, 0.05f); 

                switch (matriz[i, j])
                {
                    case 0: 
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        colorBloque = new Color(
                            Mathf.Clamp01(0.0f + variation),
                            Mathf.Clamp01(0.3f + variation),
                            Mathf.Clamp01(0.7f + variation)
                        );
                        break;

                    case 5: 
                    case 6:
                    case 7:
                    case 8:
                        colorBloque = new Color(
                            Mathf.Clamp01(0.2f + variation),
                            Mathf.Clamp01(0.6f + variation),
                            Mathf.Clamp01(0.2f + variation)
                        );
                        break;

                    case 9:
                    case 10:
                    case 11:
                    case 12:
                        colorBloque = new Color(
                            Mathf.Clamp01(0.4f + variation),
                            Mathf.Clamp01(0.3f + variation),
                            Mathf.Clamp01(0.2f + variation)
                        );
                        break;

                    case 13:
                    case 14:
                    case 15:
                    case 16:
                        colorBloque = new Color(
                            Mathf.Clamp01(0.9f + variation),
                            Mathf.Clamp01(0.9f + variation),
                            Mathf.Clamp01(1.0f)
                        );
                        break;

                    default:
                        colorBloque = Color.magenta;
                        Debug.Log("Color no definido para el bloque: " + matriz[i, j]);
                        break;
                }

                bloque.GetComponent<Renderer>().material.color = colorBloque;
            }
        }
    }

}
