using System;
using Interfaces;
using ScriptableObjects.Ability;
using UnityEngine;

namespace Player
{
    public class AbilityHandler : MonoBehaviour, ITargetAcquirer
    {
        #region Property and Variables

        private const int NumberOfAbility = 5;
        [SerializeField] private AbilitySo[] abilities = new AbilitySo[NumberOfAbility];

        public AbilitySo SelectedAbility { get; private set; }

        #endregion

        #region Events

        public Action<int> OnAbilityPress;
        public Action<int> OnAbilityPressRelease;
        public Action OnLeftMousePress;
        public Action OnLeftMousePressRelease;

        #endregion

        #region Various needed get to be implemented

        public Transform ShootingPosition { get; }
        public Vector3 AimingDirection { get; }
        public Vector3 TargetPosition { get; }

        public Transform TargetTransform { get; set; }
        public int Gold { get; }

        #endregion

        private void Awake()
        {
            InitAbilities();
        }

        private void Update()
        {
            CheckDefaultAbilityAutoSelection();
            RefreshAbilities();
        }

        #region Privat Methods (Hidden Complexity)

        private void InitAbilities()
        {
            for (var i = 0; i < NumberOfAbility; i++)
            {
                if (!abilities[i]) continue;
                abilities[i] = Instantiate(abilities[i]);
                abilities[i].Init(this);
            }
        }

        private void CheckDefaultAbilityAutoSelection()
        {
            if (SelectedAbility.IsOnCooldown && SelectedAbility != abilities[0])
                SelectedAbility = abilities[0];
        }

        private void RefreshAbilities()
        {
            foreach (var ability in abilities)
                if (ability)
                    ability.Refresh();
        }

        private void OnLeftMousePressEvent()
        {
            if (SelectedAbility)
                SelectedAbility.OnFirePress?.Invoke();
        }

        private void OnLeftMousePressReleaseEvent()
        {
            if (SelectedAbility)
                SelectedAbility.OnFireRelease?.Invoke();
        }

        private void OnAbilityPressEvent(int i)
        {
            if (abilities[i] is null || SelectedAbility && SelectedAbility.IsChanneling) return;

            //Cancel Spell
            if (abilities[i] == SelectedAbility)
            {
                SelectedAbility = abilities[0];
                return;
            }

            if (!abilities[i].IsReadyToCast) return;
            SelectedAbility = abilities[i];
            SelectedAbility.OnFirePress?.Invoke();
        }

        #endregion
    }
}