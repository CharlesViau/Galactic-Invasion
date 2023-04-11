using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemies
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private float _scaling;

        [SerializeField] private float timeBetweenWaves;
        [SerializeField] private List<EnemySpawner> _spawnerList;
        [SerializeField] private List<EnemySpawner> _slowSpawnerList;
        [SerializeField] private List<EnemySpawner> _fastSpawnerList;
        [SerializeField] private List<WaveScriptableObject> _wavesData;
        private List<EnemySpawner> _activeFastSpawners;
        private List<EnemySpawner> _activeSlowSpawners;
        private List<EnemySpawner> _activeSpawners;
        private int _currentWave;
        private bool _gameStarted;
        private List<int> _spawnOrder;
        private float _time;

        private int emptySpawners;

        private void Start()
        {
            _activeSpawners = new List<EnemySpawner>();
            _activeSlowSpawners = new List<EnemySpawner>();
            _activeFastSpawners = new List<EnemySpawner>();
        }

        private void OnDestroy()
        {
            ClearQueue();
        }

        public void GameStart()
        {
            _gameStarted = true;
            _currentWave = 0;
            NextWave();
        }

        private void NextWave()
        {
            if (_currentWave >= _wavesData.Count)
            {
                Endgame();
                return;
            }

            var data = _wavesData[_currentWave];
            if (data.normal.newSpawner)
                if (_spawnerList.Count > 0)
                {
                    _activeSpawners.Add(_spawnerList[0]);
                    _spawnerList[0].gameObject.SetActive(true);
                    _spawnerList[0].noMoreEnemyEvent += SpawnerEmpty;
                    _spawnerList.RemoveAt(0);
                }

            if (data.fast.newSpawner)
                if (_fastSpawnerList.Count > 0)
                {
                    _activeFastSpawners.Add(_fastSpawnerList[0]);
                    _fastSpawnerList[0].gameObject.SetActive(true);
                    _fastSpawnerList[0].noMoreEnemyEvent += SpawnerEmpty;
                    _fastSpawnerList.RemoveAt(0);
                }

            if (data.slow.newSpawner)
                if (_slowSpawnerList.Count > 0)
                {
                    _activeSlowSpawners.Add(_slowSpawnerList[0]);
                    _slowSpawnerList[0].gameObject.SetActive(true);
                    _slowSpawnerList[0].noMoreEnemyEvent += SpawnerEmpty;
                    _slowSpawnerList.RemoveAt(0);
                }

            StartCoroutine(TimerBetweenWaves(data.normal, data.fast, data.slow));
        }

        private void DistributeEnemies(EnemyData normal, EnemyData fast, EnemyData slow)
        {
            if (normal.numberOfEnemies > 0 && _activeSpawners.Count > 0)
            {
                var normalPerSpawner = normal.numberOfEnemies / _activeSpawners.Count;
                foreach (var s in _activeSpawners)
                {
                    s.spawnCooldown = normal.spawnCooldown;

                    for (var i = 0; i < normalPerSpawner; i++) s.AddEnemy(EnemyTypes.Asteroid);
                }
            }

            if (fast.numberOfEnemies > 0 && _activeFastSpawners.Count > 0)
            {
                var fastPerSpawner = fast.numberOfEnemies / _activeFastSpawners.Count;
                foreach (var s in _activeFastSpawners)
                {
                    s.spawnCooldown = fast.spawnCooldown;

                    for (var i = 0; i < fastPerSpawner; i++) s.AddEnemy(EnemyTypes.AsteroidFast);
                }
            }

            if (fast.numberOfEnemies > 0 && _activeSlowSpawners.Count > 0)
            {
                var slowPerSpawner = slow.numberOfEnemies / _activeSlowSpawners.Count;
                foreach (var s in _activeSlowSpawners)
                {
                    s.spawnCooldown = slow.spawnCooldown;

                    for (var i = 0; i < slowPerSpawner; i++) s.AddEnemy(EnemyTypes.AsteroidSlow);
                }
            }
        }

        private void Endgame()
        {
            var data = _wavesData.Last();
            var enemyScaling = (_currentWave - (_wavesData.Count - 2)) * _scaling;
            var rateScaling = enemyScaling / 85 + 1;
            var scaledNormal = new EnemyData();
            var scaledFast = new EnemyData();
            var scaledSlow = new EnemyData();
            scaledNormal.numberOfEnemies = (int)(data.normal.numberOfEnemies * enemyScaling);
            scaledNormal.spawnCooldown = data.normal.spawnCooldown / rateScaling;
            scaledFast.numberOfEnemies = (int)(data.fast.numberOfEnemies * enemyScaling);
            scaledFast.spawnCooldown = data.fast.spawnCooldown / rateScaling;
            scaledSlow.numberOfEnemies = (int)(data.slow.numberOfEnemies * enemyScaling);
            scaledSlow.spawnCooldown = data.slow.spawnCooldown / rateScaling;
            StartCoroutine(TimerBetweenWaves(scaledNormal, scaledFast, scaledSlow));
        }

        private void SpawnerEmpty()
        {
            emptySpawners++;
            if (emptySpawners == _activeSpawners.Count + _activeFastSpawners.Count + _activeSlowSpawners.Count)
            {
                emptySpawners = 0;
                NextWave();
            }
        }

        public void ClearQueue()
        {
            foreach (var spawner in _activeSpawners)
            {
                spawner.noMoreEnemyEvent -= SpawnerEmpty;
                spawner.ClearQueue();
            }

            foreach (var spawner in _activeFastSpawners)
            {
                spawner.noMoreEnemyEvent -= SpawnerEmpty;
                spawner.ClearQueue();
            }

            foreach (var spawner in _activeSlowSpawners)
            {
                spawner.noMoreEnemyEvent -= SpawnerEmpty;
                spawner.ClearQueue();
            }
        }

        private IEnumerator TimerBetweenWaves(EnemyData normal, EnemyData fast, EnemyData slow)
        {
            if (_currentWave == 0)
                yield return new WaitForSeconds(1);
            else
                yield return new WaitForSeconds(timeBetweenWaves);

            DistributeEnemies(normal, fast, slow);
            _currentWave++;
        }
    }
}