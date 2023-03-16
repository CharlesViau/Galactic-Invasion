using System;
using Enemies;
using UnityEngine;

namespace Motherbase
{
    public class PlayerCurrency : MonoBehaviour
    {
        public int balance;

        private void Start()
        {
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
        }

        private void AddMoney(int amount)
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