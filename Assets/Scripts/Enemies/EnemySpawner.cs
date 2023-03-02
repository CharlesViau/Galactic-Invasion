using Enemies;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnCooldown;

    private int numberOfEnemiesToSpawn;

    private float time;
    
    private void Start()
    {
        //Play animation
    }

    private void FixedUpdate()
    {
        if (numberOfEnemiesToSpawn > 0)
        {
            time += Time.deltaTime;
            if (time >= spawnCooldown)
            {
                time = 0;
                Spawn();
                numberOfEnemiesToSpawn -= 1;
            }
        }
    }

    public void AddEnemy()
    {
        numberOfEnemiesToSpawn++;
    }

    private void Spawn()
    {
        // ReSharper disable once Unity.InefficientPropertyAccess
        EnemyManager.Instance.Create(EnemyTypes.Asteroid, new Enemy.Args(transform.position, transform.rotation));
    }
}
