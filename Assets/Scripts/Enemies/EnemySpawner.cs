using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnCooldown;
    private List<EnemyTypes> _enemyQueue;
    private float _time;

    private void Awake()
    {
        _enemyQueue = new List<EnemyTypes>();
    }

    private void FixedUpdate()
    {
        if (_enemyQueue.Count > 0)
        {
            _time += Time.deltaTime;
            if (_time >= spawnCooldown)
            {
                _time = 0;
                Spawn(_enemyQueue[0]);
                _enemyQueue.RemoveAt(0);
            }
        }
    }

    public void AddEnemy(EnemyTypes type)
    {
        _enemyQueue.Add(type);
    }

    private void Spawn(EnemyTypes type)
    {
        // ReSharper disable once Unity.InefficientPropertyAccess
        EnemyManager.Instance.Create(type, new Enemy.Args(transform.position, transform.rotation));
    }

    public void ClearQueue()
    {
        _enemyQueue.Clear();
    }
}