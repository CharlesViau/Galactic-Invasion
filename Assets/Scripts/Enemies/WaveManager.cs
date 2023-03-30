using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private float scaling;

        [SerializeField] private int waveCost;

        [SerializeField] private int normalCost;

        [SerializeField] private int fastCost;

        [SerializeField] private int slowCost;

        [SerializeField] private float timeBetweenWaves;

        private int _currentWave;

        private float _fastPercentage = 0.10f;

        private int _numberOfDegreesBetweenSpawners;

        private float _slowPercentage = 0.10f;

        [SerializeField] private List<EnemySpawner> _spawnerList;

        private List<EnemySpawner> _activeSpawners;

        private List<int> _spawnOrder;

        private float _time;

        private float slowPercentage = 0.10f;

        private bool gameStarted = false;
        
        private void Start()
        {
            _activeSpawners = new List<EnemySpawner>();
            _spawnOrder = new List<int>();
            for (var i = 0; i < _spawnerList.Count; i++) _spawnOrder.Add(i);
            _spawnOrder = Shuffle(_spawnOrder);
            NewSpawner();
        }

        private void FixedUpdate()
        {
            if (gameStarted)
            {
                _time += Time.deltaTime;
                    
                if (_time >= timeBetweenWaves)
                {
                    NextWave();
                    _time = 0;
                }
            }
            if (!Controller.Instance.gameStarted) return;
            _time += Time.deltaTime;

            if (!(_time >= timeBetweenWaves)) return;
            NextWave();
            _time = 0;
        }

        public void GameStart()
        {
            gameStarted = true;
            _currentWave = 1;
            NextWave();
        }

        private void NextWave()
        {
            switch (_currentWave % 5)
            {
                case 0:
                    NewSpawner();
                    break;
                case 4:
                    MessageUI.Instance.SetText("New Spawner Incoming!");
                    MessageUI.Instance.Show();
                    break;
            }

            _currentWave++;
            PickEnemies();
            waveCost = Mathf.FloorToInt(waveCost * scaling);
        }

        private void PickEnemies()
        {
            var numberOfFast = Mathf.FloorToInt(waveCost * _fastPercentage / fastCost);
            var numberOfSlow = Mathf.FloorToInt(waveCost * _slowPercentage / slowCost);
            var numberOfNormal =
                Mathf.FloorToInt((waveCost - (numberOfFast * fastCost + numberOfSlow * slowCost)) / normalCost);

            var enemies = new List<EnemyTypes>();

            for (var i = 0; i < numberOfNormal; i++) enemies.Add(EnemyTypes.Asteroid);

            for (var i = 0; i < numberOfFast; i++) enemies.Add(EnemyTypes.AsteroidFast);

            for (var i = 0; i < numberOfSlow; i++) enemies.Add(EnemyTypes.AsteroidSlow);

            enemies = Shuffle(enemies);

            _fastPercentage += 0.01f;
            _slowPercentage += 0.01f;

            foreach (var t in enemies)
                GetRandomSpawner().AddEnemy(t);
        }

        private void NewSpawner()
        {
            if (_spawnOrder.Count > 0)
            {
                _spawnerList[_spawnOrder[0]].gameObject.SetActive(true);
                _activeSpawners.Add(_spawnerList[_spawnOrder[0]]);
                _spawnOrder.RemoveAt(0);
            }
        }

        private EnemySpawner GetRandomSpawner()
        {
            return _activeSpawners[Random.Range(0, _activeSpawners.Count)];
        }

        private List<T> Shuffle<T>(List<T> data)
        {
            var rng = new System.Random();
            var n = data.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                (data[k], data[n]) = (data[n], data[k]);
            }

            return data;
        }
    }
}