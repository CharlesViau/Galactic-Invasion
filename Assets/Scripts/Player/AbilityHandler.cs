using System;
using Interfaces;
using Motherbase;
using ScriptableObjects.Ability;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class AbilityHandler : MonoBehaviour, ITargetAcquirer
    {
        #region Property and Variables

        private const int NumberOfAbility = 3;
        [SerializeField] private AbilitySo[] abilities = new AbilitySo[NumberOfAbility];

        public AbilitySo SelectedAbility { get; private set; }

        #endregion

        #region Various needed get to be implemented

        private PlayerReferencing _referencing;

        public Transform ShootingPosition => _referencing.referenceTransform;
        public Vector3 AimingDirection { get; }
        public Vector3 TargetPosition => _referencing.referenceTransform.position;

        public Transform TargetTransform { get; set; }
        public static int Gold => PlayerCurrency.Instance.Balance;

        private bool _isInDesigningDefenseMode;

        #endregion

        private void OnEnable()
        {
            PlayerInputEventHandler.Instance.OnAbilityPerform += OnAbilityPressEvent;
            PlayerInputEventHandler.Instance.OnLeftClickPerform += OnLeftMousePress;
            PlayerInputEventHandler.Instance.OnLeftClickCancel += OnLeftMousePressRelease;
        }

        private void OnDisable()
        {
            PlayerInputEventHandler.Instance.OnAbilityPerform -= OnAbilityPressEvent;
            PlayerInputEventHandler.Instance.OnLeftClickPerform -= OnLeftMousePress;
            PlayerInputEventHandler.Instance.OnLeftClickCancel -= OnLeftMousePressRelease;
        }

        private void Awake()
        {
            InitAbilities();

            _referencing = GetComponent<PlayerReferencing>();
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

            SelectedAbility = abilities[0];
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

        private void OnLeftMousePress()
        {
            if (SelectedAbility)
                SelectedAbility.OnFirePress?.Invoke();
        }

        private void OnLeftMousePressRelease()
        {
            if (SelectedAbility)
                SelectedAbility.OnFireRelease?.Invoke();
        }

        private void OnAbilityPressEvent(int i)
        {
            if (i > abilities.Length - 1) return;
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