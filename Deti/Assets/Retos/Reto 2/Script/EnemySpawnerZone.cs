using UnityEngine;
using System.Collections.Generic;
public class EnemySpawnerZone : MonoBehaviour
{
    // =============================================
    // CONFIGURACIÓN DEL SPAWN (Visible en el Inspector)
    // =============================================
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;  // Prefab del enemigo a generar
    [SerializeField] private int maxEnemies = 5;     // Máximo de enemigos activos simultáneamente
    [SerializeField] private float spawnInterval = 2f; // Tiempo entre generación de enemigos
    [SerializeField] private float spawnRadius = 3f;  // Radio donde aparecerán los enemigos
    // =============================================
    // REFERENCIAS
    // =============================================
    [Header("References")]
    [SerializeField] private Transform player;  // Referencia al jugador
    // =============================================
    // VARIABLES PRIVADAS DE ESTADO
    // =============================================
    private bool playerInZone = false;      // ¿Está el jugador en la zona?
    private float spawnTimer = 0f;          // Temporizador para controlar intervalos
    private List<GameObject> activeEnemies = new List<GameObject>(); // Lista de enemigos activos
    // =============================================
    // INICIALIZACIÓN
    // =============================================
    private void Awake()
    {
        // Buscar automáticamente al jugador si no está asignado
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    // =============================================
    // ACTUALIZACIÓN POR FRAME
    // =============================================
    private void Update()
    {
        // Solo generar enemigos si:
        // 1. El jugador está en la zona
        // 2. No hemos alcanzado el máximo de enemigos
        if (playerInZone && activeEnemies.Count < maxEnemies)
        {
            spawnTimer += Time.deltaTime;
            
            // Cuando pasa el intervalo de tiempo...
            if (spawnTimer >= spawnInterval)
            {
                SpawnEnemy();  // Generar nuevo enemigo
                spawnTimer = 0f; // Reiniciar temporizador
            }
        }
    }

    // =============================================
    // GENERACIÓN DE ENEMIGOS
    // =============================================
    private void SpawnEnemy()
    {
        if (enemyPrefab == null) return; // Seguridad si no hay prefab

        // Calcular posición aleatoria alrededor del spawner
        Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
        randomOffset.y = 0; // Mantener en el suelo (eje Y)
        
        Vector3 spawnPosition = transform.position + randomOffset;

        // Instanciar nuevo enemigo y añadirlo a la lista
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        activeEnemies.Add(newEnemy);
    }

    // =============================================
    // DETECCIÓN DE JUGADOR EN LA ZONA
    // =============================================
    private void OnTriggerEnter(Collider other)
    {
        // Cuando el jugador entra al trigger...
        if (other.transform == player)
        {
            playerInZone = true; // Activar bandera
            spawnTimer = 0f;     // Reiniciar temporizador
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cuando el jugador sale del trigger...
        if (other.transform == player)
        {
            playerInZone = false; // Desactivar generación
            // Nota: Los enemigos existentes permanecen
        }
    }

    // =============================================
    // LIMPIEZA DE ENEMIGOS DESTRUIDOS
    // =============================================
    private void LateUpdate()
    {
        // Recorrer lista de enemigos al revés para poder eliminar elementos
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] == null) // Si el enemigo fue destruido...
            {
                activeEnemies.RemoveAt(i); // Eliminar de la lista
            }
        }
    }

    // =============================================
    // VISUALIZACIÓN EN EL EDITOR (DEBUG)
    // =============================================
    private void OnDrawGizmosSelected()
    {
        // Dibujar radio de spawn (cyan)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        
        // Dibujar zona de trigger (amarillo) si existe un SphereCollider
        if (GetComponent<SphereCollider>() != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>().radius);
        }
    }
}