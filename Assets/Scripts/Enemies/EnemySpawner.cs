using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnCooldown;

    private List<EnemyTypes> enemyQueue;

    private float time;
    
    private void Awake()
    {
        enemyQueue = new List<EnemyTypes>();
    }

    private void FixedUpdate()
    {
        if (enemyQueue.Count > 0)
        {
            time += Time.deltaTime;
            if (time >= spawnCooldown)
            {
                time = 0;
                Spawn(enemyQueue[0]);
                enemyQueue.RemoveAt(0);
            }
        }
    }

    public void AddEnemy(EnemyTypes type)
    {
        enemyQueue.Add(type);
    }

    private void Spawn(EnemyTypes type)
    {
        // ReSharper disable once Unity.InefficientPropertyAccess
        EnemyManager.Instance.Create(type, new Enemy.Args(transform.position, transform.rotation));
    }
}
