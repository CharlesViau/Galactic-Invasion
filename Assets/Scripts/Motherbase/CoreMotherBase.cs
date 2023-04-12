using System.Collections.Generic;
using System.Linq;
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
        private int _currentLvl = 1;
        private Color _finalColor;
        private bool _isPreviewsShown;
        private Color _lerpedColor;
        private Material _material;
        private Color _midColor;
        private List<int> _spawnedShields;
        private Color _startingColor;
        public AK.Wwise.Event onDeath;
        public AK.Wwise.Event stopMusic;

        private void Awake()
        {
            _material = gameObject.GetComponent<Renderer>().material;
            _startingColor = new Color((float)37 / 255, (float)150 / 255, (float)29 / 255, 1f);
            _midColor = new Color((float)191 / 255, (float)191 / 255, (float)191 / 255, 1f);
            _lerpedColor = _startingColor;
            _finalColor = new Color((float)205 / 255, (float)23 / 255, (float)23 / 255, 1f);
            UpdateColor(1);
            _spawnedShields = new List<int>();

            for (var i = 0; i < shields_preview.Count; i++) shields_preview[i].SetIndex(i);

            foreach (var s in shields)
            {
                s.GetTowerReferences();
                s.deathEvent += OnShieldDestroy;
            }
        }

        private void Update()
        {
            if (!_isPreviewsShown) return;
            ShowShieldsPreview(_isPreviewsShown);
        }

        private void OnDestroy()
        {

            stopMusic?.Post(gameObject);
            onDeath?.Post(gameObject);

            foreach (var s in shields) s.deathEvent -= OnShieldDestroy;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (!collider.CompareTag("Enemy")) return;
            var enemy = collider.gameObject.GetComponent<Enemy>();
            ReceiveDamage(enemy.damage);
            if (!enemy.isPoolable)
                Destroy(enemy.gameObject);
            else
                EnemyManager.Instance.Pool(collider.gameObject.GetComponent<Enemy>());
        }

        private void ReceiveDamage(int dmg)
        {
            hp -= dmg;

            if (hp <= 0) GameOver();
        }

        private void GameOver()
        {
            onDeath.Post(gameObject);
            EnemyManager.Instance.Clear();
            ProjectileManager.Instance.Clear();
            Controller.Instance.Reset();
            SceneManager.LoadScene("GameOverScene");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void DestroyShields()
        {
            foreach (var s in shields) s.RingDestroyed();
        }

        public void ShowShieldsPreview(bool show)
        {
            _isPreviewsShown = show;

            for (var i = 0; i < shields_preview.Count; i++)
                if (!_spawnedShields.Contains(i))
                    shields_preview[i].gameObject.SetActive(show);
        }

        public void ShowFirstShieldPreview(bool show)
        {
            shields_preview[0].gameObject.SetActive(show);
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

        public void ShieldsSelectable(bool selectable, bool isUpgrading = false)
        {
            foreach (var s in shields.Where(s => !isUpgrading || !s.IsMaxLvl())) s.isSelectable = selectable;
        }

        private void OnShieldDestroy()
        {
            for (var i = 0; i < shields.Count; i++)
                if (!shields[i].gameObject.activeSelf && _spawnedShields.Contains(i))
                    _spawnedShields.Remove(i);
        }

        public void UpdateColor(float ringHpPercentage)
        {
            if (ringHpPercentage >= 0.5f)
                _lerpedColor = Color.Lerp(_startingColor, _midColor, (1 - ringHpPercentage) * 2);
            else
                _lerpedColor = Color.Lerp(_finalColor, _midColor, ringHpPercentage * 2);

            _material.SetColor("_EmissionColor", _lerpedColor * 2.5f);
        }
    }
}