using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    
    [SerializeField] private EnemySpawner spawnerPrefab;

    [SerializeField] private short numberOfSpawner;

    [SerializeField] private float portalDistanceFromOrigin;

    [SerializeField] private float scaling;

    [SerializeField] private int waveCost;

    [SerializeField] private float timeBetweenWaves;

    private float time;

    private int numberOfDegreesBetweenSpawners;

    private List<int> spawnOrder;

    private int currentWave;

    private List<EnemySpawner> spawnerList;
    
    private void Start()
    {
        spawnerList = new List<EnemySpawner>();
        spawnOrder = new List<int>();
        numberOfDegreesBetweenSpawners = 360 / numberOfSpawner;
        for (int i = 0; i < numberOfSpawner; i++)
        {
            spawnOrder.Add(numberOfDegreesBetweenSpawners * i);
        }
        Shuffle();
        NewSpawner();
        NewSpawner();
        GameStart();
    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;

        if (time >= timeBetweenWaves)
        {
            NextWave();
            time = 0;
        }
    }
    
    private void GameStart()
    {
        currentWave = 1;
        NextWave();
    }

    private void NextWave()
    {
        if (currentWave % 5 == 0)
        {
            NewSpawner();
        }
        currentWave++;
        PickEnemies();
    }

    private void PickEnemies()
    {
        waveCost = Mathf.FloorToInt(waveCost * scaling);

        for (int i = 0; i < waveCost; i++)
        {
            GetRandomSpawner().AddEnemy();
        }
    }

    private void NewSpawner()
    {
        //After X spawner spawned, the next will be closer
        if (spawnOrder.Count > 0)
        {
            Vector3 v = Quaternion.AngleAxis(spawnOrder[0], Vector3.forward) * Vector3.up;
            Ray ray = new Ray(Vector3.zero, v);

            EnemySpawner spawner = Instantiate(spawnerPrefab, ray.GetPoint(portalDistanceFromOrigin), Quaternion.identity);
            spawnerList.Add(spawner);
            
            spawnOrder.RemoveAt(0);
        }
    }

    private EnemySpawner GetRandomSpawner()
    {
        return spawnerList[Random.Range(0, spawnerList.Count)];
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
