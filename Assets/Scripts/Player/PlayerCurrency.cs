using System;
using Enemies;
using UnityEngine;

namespace Motherbase
{
    public class PlayerCurrency : MonoBehaviour
    {
        [SerializeField] private int balance;
        public int shieldCost;
        public int repairCost;
        public int upgradeCost;
        public int blackHoleCost;
        public int tempoPlanetCost;
        public int Balance => balance;
        public static PlayerCurrency Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            EnemyManager.OnEnemyDeath += OnEnemyDeath;
        }

        private void OnDisable()
        {
            EnemyManager.OnEnemyDeath -= OnEnemyDeath;
        }

        public event Action<int> OnBalanceChanged;

        private void OnEnemyDeath(int reward)
        {
            AddMoney(reward);
            PlayerScore.Instance.AddScore(1);
        }

        public void AddMoney(int amount)
        {
            balance += amount;
            OnBalanceChanged?.Invoke(balance);
        }

        public bool SpendMoney(int amount)
        {
            if (balance < amount) return false;
            balance -= amount;
            OnBalanceChanged?.Invoke(balance);
            return true;
        }
    }
}