using System.Collections;
using UnityEngine;

public class CaminataBorracha : MonoBehaviour
{
    private int[,] mapMatrix = new int[100, 100];
    public GameObject cubo;
    public Material cuboMaterial;







    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AlgoritmoBorracho();
        CrearCubos();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AlgoritmoBorracho()
    {
        int coordenadaX = Random.Range(0, 100);
        int coordenadaY = Random.Range(0, 100);
        float step = mapMatrix.Length*0.25f;
        

        for (int i = 0; i < step; i++)
        {
            int pasoRandom = Random.Range(0, 4);
            switch (pasoRandom)
            {
                case 0: //ARRIBA
                    if (coordenadaY > 0)
                    {
                        mapMatrix[coordenadaX, coordenadaY] +=1;
                        coordenadaY--;
                    }
                    break;

                case 1: //DERECHA
                    if (coordenadaX < mapMatrix.GetLength(0)-1)
                    {
                        mapMatrix[coordenadaX, coordenadaY] +=1;
                        coordenadaX++;
                    }

                    break;

                case 2: //IZQUIERDA
                    if (coordenadaY < mapMatrix.GetLength(1) - 1)
                    {
                        mapMatrix[coordenadaX, coordenadaY] +=1;
                        coordenadaY++;
                    }
                    break;

                case 3://ABAJO
                    if (coordenadaX > 0)
                    {
                        mapMatrix[coordenadaX, coordenadaY] +=1;
                        coordenadaX--;
                    }
                    break;
            }


        }


    }

    void CrearCubos()
    {
        for (int i = 0; i < mapMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < mapMatrix.GetLength(1); j++) 
            {
                if (mapMatrix[i, j] != 0)
                {
                    Vector3 posicion = new Vector3(i, 0, j);
                    GameObject cuboMaterial = Instantiate(cubo, posicion, Quaternion.identity);
                    Renderer renderer = cuboMaterial.GetComponent<Renderer>();
                    switch (mapMatrix[i,j])
                    {
                        case 1:
                            renderer.material.color = Color.blue;
                            break;

                        case 2:
                            renderer.material.color = Color.green;
                            break;

                        case 3:
                            renderer.material.color = Color.yellow;

                            break;

                        case 4:
                            renderer.material.color = new Color(1,0.65f,0);
                            break;

                        case 5:
                            renderer.material.color = Color.red;
                            break;

                        case 6:
                            renderer.material.color = Color.black;
                            break;

                        default:
                            renderer.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0.5f);
                            break;



                    }
                }
            
            }
        
        }
    }
}
