using Enemies;
using Towers;
using UnityEngine;
using UnityEngine.UI;

namespace Motherbase
{
    public class Shield : MonoBehaviour
    {
        public delegate void DeathEvent();

        public bool isSelectable;
        [SerializeField] private int maxHP;
        [SerializeField] private Transform t;
        [SerializeField] private Image healthBar;
        private Color _baseColor;
        private int _currentLvl = 1;
        private Vector3 _direction;
        private float _gravityStrength;
        private int _hp;
        private bool _imageDestroyed = false;
        private bool _isRingDestroyed;
        private Material _material;
        private Transform _rocket;
        private Color _selectedColor;
        private Transform _sniper;
        private Tower _tower;
        private Vector3 _velocity;

        private void Reset()
        {
            healthBar.gameObject.transform.parent.gameObject.SetActive(true);
            _hp = maxHP;
            UpdateHealthBar(_hp, maxHP);
            _currentLvl = 0;
            Upgrade();
        }

        private void Awake()
        {
            _velocity = Vector3.zero;
            _baseColor = new Color((float)188 / 255, (float)173 / 255, (float)173 / 255, 1f);
            _selectedColor = new Color((float)102 / 255, (float)212 / 255, (float)75 / 255, 1f);
            _material = gameObject.GetComponent<Renderer>().material;
        }

        private void FixedUpdate()
        {
            if (!_isRingDestroyed) return;
            var gravityForce = _direction * _gravityStrength;
            _velocity = _velocity + gravityForce * Time.deltaTime;
            transform.position = transform.position + _velocity * Time.deltaTime +
                                 gravityForce / 2 * Mathf.Pow(Time.deltaTime, 2);
        }

        private void OnEnable()
        {
            Reset();
        }

        private void OnMouseEnter()
        {
            if (isSelectable)
                _material.color = _selectedColor;
        }

        private void OnMouseExit()
        {
            if (isSelectable)
                _material.color = _baseColor;
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

        public event DeathEvent deathEvent;

        public void GetTowerReferences()
        {
            _tower = transform.Find("Tower_lvl_one").GetComponent<Tower>();
            _rocket = transform.Find("Tower_lvl_one/tourelle01_v03/tourelleRotation_grp/arme_grp/rocket_grp");
            _sniper = transform.Find("Tower_lvl_one/tourelle01_v03/tourelleRotation_grp/arme_grp/sniper_grp");
        }

        public void Upgrade()
        {
            _currentLvl++;
            switch (_currentLvl)
            {
                case 1:
                    _rocket.gameObject.SetActive(false);
                    _sniper.gameObject.SetActive(false);
                    _tower.ResetTower();
                    break;
                case 2:
                    _rocket.gameObject.SetActive(true);
                    _tower.Upgrade(_currentLvl);
                    break;
                case 3:
                    _sniper.gameObject.SetActive(true);
                    _tower.Upgrade(_currentLvl);
                    break;
            }

            _material.color = _baseColor;
        }

        public void Repair()
        {
            _hp = maxHP;
            UpdateHealthBar(_hp, maxHP);
            _material.color = _baseColor;
        }

        public void RingDestroyed()
        {
            _isRingDestroyed = true;
            _direction = (t.position - Vector3.zero).normalized;
            _gravityStrength = Random.Range(6f, 8f);
            healthBar.gameObject.transform.parent.gameObject.SetActive(false);
        }

        private void ReceiveDamage(int dmg)
        {
            _hp -= dmg;

            if (_hp <= 0)
            {
                gameObject.SetActive(false);
                //Destruction animation
                deathEvent?.Invoke();
                healthBar.gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                UpdateHealthBar(_hp, maxHP);
            }
        }

        private void UpdateHealthBar(int currentHp, int maxHp)
        {
            var healthPercentage = (float)currentHp / maxHp;
            healthBar.fillAmount = healthPercentage;
        }

        public bool IsMaxLvl()
        {
            return _currentLvl >= 3;
        }

        public bool IsMaxHp()
        {
            return _hp >= maxHP;
        }
    }
}