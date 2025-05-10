using UnityEngine;

public class camaratopdown : MonoBehaviour
{
    [SerializeField] private Transform player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y + 5f, player.position.z - 5f);
        transform.LookAt(player.position);
    }
}
