using System;
using Ability;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Ability
{
    public abstract class AbilityTargetingStateSo : ScriptableObject
    {
        [SerializeField]private TargetingSo targetingSo;
        protected TargetingSo TargetingSoClone;
        protected AbilitySo AbilitySo;
        [HideInInspector] public SpellUIType spellUIType;

        public Action OnFirePress;
        public Action OnFireRelease;

        public virtual void Init(AbilitySo abilitySo)
        {
            AbilitySo = abilitySo;
            OnFirePress = OnFirePressEvent;
            OnFireRelease = OnFireReleaseEvent;
            spellUIType = abilitySo.spellUIType;

            TargetingSoClone = Instantiate(targetingSo);
            TargetingSoClone.Init(abilitySo);
        }

        public abstract void Refresh();

        public abstract void OnEnter();

        public abstract void OnExit();

        protected abstract void OnFirePressEvent();
        protected abstract void OnFireReleaseEvent();
    }
}