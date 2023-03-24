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

        private int _currentWave;

        private float _fastPercentage = 0.10f;

        private int _numberOfDegreesBetweenSpawners;

        private float _slowPercentage = 0.10f;

        private List<EnemySpawner> _spawnerList;

        private List<int> _spawnOrder;

        private float _time;

        private void Start()
        {
            _spawnerList = new List<EnemySpawner>();
            _spawnOrder = new List<int>();
            _numberOfDegreesBetweenSpawners = 360 / numberOfSpawner;
            for (var i = 0; i < numberOfSpawner; i++) _spawnOrder.Add(_numberOfDegreesBetweenSpawners * i);
            _spawnOrder = Shuffle(_spawnOrder);
            NewSpawner();
        }

        private void FixedUpdate()
        {
            if (!Controller.Instance.gameStarted) return;
            _time += Time.deltaTime;

            if (!(_time >= timeBetweenWaves)) return;
            NextWave();
            _time = 0;
        }

        public void GameStart()
        {
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
                var v = Quaternion.AngleAxis(_spawnOrder[0], Vector3.forward) * Vector3.up;
                var ray = new Ray(Vector3.zero, v);

                var spawner = Instantiate(spawnerPrefab, ray.GetPoint(portalDistanceFromOrigin), Quaternion.identity);
                _spawnerList.Add(spawner);

                _spawnOrder.RemoveAt(0);
            }
        }

        private EnemySpawner GetRandomSpawner()
        {
            return _spawnerList[Random.Range(0, _spawnerList.Count)];
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