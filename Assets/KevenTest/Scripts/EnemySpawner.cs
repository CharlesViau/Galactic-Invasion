using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        Enemy instance = Instantiate(enemy, transform.position, transform.rotation);
    }
}
