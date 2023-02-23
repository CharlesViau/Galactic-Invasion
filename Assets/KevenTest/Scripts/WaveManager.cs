using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    
    [SerializeField] private EnemySpawner spawnerPrefab;

    [SerializeField] private short numberOfSpawner;

    [SerializeField] private float distanceFromOrigin;

    private int numberOfDegreesBetweenSpawners;

    private List<int> spawnOrder;
    
    private void Start()
    {
        spawnOrder = new List<int>();
        numberOfDegreesBetweenSpawners = 360 / numberOfSpawner;
        for (int i = 0; i < numberOfSpawner; i++)
        {
            spawnOrder.Add(numberOfDegreesBetweenSpawners * i);
        }
        Shuffle();
        newSpawner();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            newSpawner();
        }
    }
    private void GameStart()
    {
        
    }

    private void NextWave()
    {
        
    }

    private void PickEnemies()
    {
        
    }

    private void newSpawner()
    {
        if (spawnOrder.Count > 0)
        {
            Vector3 v = Quaternion.AngleAxis(spawnOrder[0], Vector3.forward) * Vector3.up;
            Ray ray = new Ray(Vector3.zero, v);

            Instantiate(spawnerPrefab, ray.GetPoint(distanceFromOrigin), Quaternion.identity);
            
            spawnOrder.RemoveAt(0);
        }
    }
    
    private void Shuffle()
    {
        System.Random rng = new System.Random();
        int n = spawnOrder.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            int value = spawnOrder[k];  
            spawnOrder[k] = spawnOrder[n];  
            spawnOrder[n] = value;  
        }  
    }
}
