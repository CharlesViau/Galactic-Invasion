using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class WaveManager : MonoBehaviour
    {
        
        [SerializeField] private EnemySpawner spawnerPrefab;
    
        [SerializeField] private short numberOfSpawner;
    
        [SerializeField] private float portalDistanceFromOrigin;
    
        [SerializeField] private float scaling;
    
        [SerializeField] private int waveCost;
        
        [SerializeField] private int normalCost;

        [SerializeField] private int fastCost;

        [SerializeField] private int slowCost;

        [SerializeField] private float timeBetweenWaves;
    
        private float time;
    
        private int numberOfDegreesBetweenSpawners;
    
        private List<int> spawnOrder;
    
        private int currentWave;
    
        private List<EnemySpawner> spawnerList;

        private float fastPercentage = 0.10f;

        private float slowPercentage = 0.10f;
        
        private void Start()
        {
            spawnerList = new List<EnemySpawner>();
            spawnOrder = new List<int>();
            numberOfDegreesBetweenSpawners = 360 / numberOfSpawner;
            for (int i = 0; i < numberOfSpawner; i++)
            {
                spawnOrder.Add(numberOfDegreesBetweenSpawners * i);
            }
            spawnOrder = Shuffle(spawnOrder);
            //StartingSpawner();
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
            waveCost = Mathf.FloorToInt(waveCost * scaling);
        }
    
        private void PickEnemies()
        {
            int numberOfFast = Mathf.FloorToInt((waveCost * fastPercentage) / fastCost);
            int numberOfSlow = Mathf.FloorToInt((waveCost * slowPercentage) / slowCost);
            int numberOfNormal = Mathf.FloorToInt((waveCost - ((numberOfFast * fastCost) + (numberOfSlow * slowCost))) / normalCost);
            
            /*Debug.Log("WaveCost: " + waveCost);
            Debug.Log("Number of fast: " + numberOfFast);
            Debug.Log("Number of slow: " + numberOfSlow);
            Debug.Log("Number of Normal: " + numberOfNormal);*/

            List<EnemyTypes> enemies = new List<EnemyTypes>();

            for (int i = 0; i < numberOfNormal; i++)
            {
                enemies.Add(EnemyTypes.Asteroid);
            }

            for (int i = 0; i < numberOfFast; i++)
            {
                enemies.Add(EnemyTypes.AsteroidFast);
            }

            for (int i = 0; i < numberOfSlow; i++)
            {
                enemies.Add(EnemyTypes.AsteroidSlow);
            }

            enemies = Shuffle(enemies);

            fastPercentage += 0.01f;
            slowPercentage += 0.01f;

            for (int i = 0; i < enemies.Count; i++)
            {
                GetRandomSpawner().AddEnemy(enemies[i]);
            }
        }
    
        private void NewSpawner()
        {
            //After X spawner spawned, the next will be closer?
            
            //2, 3 hardcoded spawner?
            
            //Divise world in 4, lock one part when spawner is spawned in it
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
        
        private List<T> Shuffle<T>(List<T> data)
        {
            System.Random rng = new System.Random();
            int n = data.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                var value = data[k];
                data[k] = data[n];
                data[n] = value;
            }

            return data;
        }
    
        private void StartingSpawner()
        {
            for (int i = 0; i < spawnOrder.Count; i++)
            {
                if (spawnOrder[i] == 0)
                {
                    int tmp = spawnOrder[0];
                    spawnOrder[0] = 0;
                    spawnOrder[i] = tmp;
                } else if (spawnOrder[i] == 180)
                {
                    int tmp = spawnOrder[1];
                    spawnOrder[1] = 180;
                    spawnOrder[i] = tmp;
                }
            }
        }
    }
}

