using System.Collections.Generic;
using Enemies;
using Projectiles;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Motherbase
{
    public class CoreMotherBase : MonoBehaviour
    {
        [SerializeField] private List<Shield> shields;
        [SerializeField] private List<ShieldPreview> shields_preview;
        [SerializeField] private int hp;
        [SerializeField] private int maxLVL;
        private int currentLVL = 1;
        private List<int> _spawnedShields;

        private void Awake()
        {
            _spawnedShields = new List<int>();

            for (var i = 0; i < shields_preview.Count; i++) shields_preview[i].SetIndex(i);

            foreach (Shield s in shields)
            {
                s.GetTowerReferences();
                s.deathEvent += OnShieldDestroy;
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Enemy"))
            {
                var enemy = collider.gameObject.GetComponent<Enemy>();
                ReceiveDamage(enemy.damage);
                if (!enemy.isPoolable)
                    Destroy(enemy.gameObject);
                else
                    EnemyManager.Instance.Pool(collider.gameObject.GetComponent<Enemy>());
            }
        }

        private void ReceiveDamage(int dmg)
        {
            hp -= dmg;

            if (hp <= 0) GameOver();
        }

        private void GameOver()
        {
            EnemyManager.Instance.Clear();
            ProjectileManager.Instance.Clear();
            Controller.Instance.Reset();
            SceneManager.LoadScene("GameOverScene");
            Cursor.visible = true;
        }

        public void DestroyShields()
        {
            foreach (var s in shields) s.RingDestroyed();
        }

        public void ShowShieldsPreview(bool show)
        {
            for (var i = 0; i < shields_preview.Count; i++)
                if (!_spawnedShields.Contains(i))
                    shields_preview[i].gameObject.SetActive(show);
        }

        public void SpawnShield(int index)
        {
            shields[index].gameObject.SetActive(true);
            _spawnedShields.Add(index);
            ShowShieldsPreview(false);
        }

        public List<Shield> GetShieldList()
        {
            return shields;
        }

        public void UpgradeShields()
        {
            currentLVL++;
            foreach (Shield s in shields)
            {
                s.Upgrade(currentLVL);
            }
        }

        public bool IsMaxLVL()
        {
            if (currentLVL >= maxLVL) return true;

            return false;
        }

        private void OnShieldDestroy()
        {
            for (var i = 0; i < shields.Count; i++)
                if (!shields[i].gameObject.activeSelf && _spawnedShields.Contains(i))
                    _spawnedShields.Remove(i);
        }

        private void OnDestroy()
        {
            foreach (Shield s in shields)
            {
                s.deathEvent -= OnShieldDestroy;
            }
        }
    }
}