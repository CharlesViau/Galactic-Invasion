using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Motherbase
{
    public class CoreMotherBase : MonoBehaviour
    {
        [SerializeField] private List<Shield> shields;
        [SerializeField] private List<ShieldPreview> shields_preview;
        [SerializeField] private int hp;

        private List<int> _spawnedShields;

        private void Start()
        {
            _spawnedShields = new List<int>();

            for (var i = 0; i < shields_preview.Count; i++) shields_preview[i].SetIndex(i);
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

            if (hp <= 0)
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            SceneManager.LoadScene("GameOverScene");
        }

        public void DestroyShields()
        {
            foreach (var s in shields) s.RingDestroyed();
        }

        public void ShowShieldsPreview(bool show)
        {
            CostUI.Instance.SetText($"Cost: {PlayerCurrency.Instance.shieldCost}");

            if (show) CostUI.Instance.Show();
            else CostUI.Instance.Hide();


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
    }
}