using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    [SerializeField] private float spawnCooldown;

    private float time;
    
    private void Start()
    {
        //Play animation
    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time >= spawnCooldown)
        {
            time = 0;
            Spawn();
        }
    }

    private void Spawn()
    {
        Instantiate(enemy, transform.position, transform.rotation);
    }
}
