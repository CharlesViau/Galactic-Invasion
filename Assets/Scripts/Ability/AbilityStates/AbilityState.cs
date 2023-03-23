using System;
using Core;
using ScriptableObjects.Ability;

namespace Ability.AbilityStates
{
    public abstract class AbilityState : State
    {
        protected readonly AbilitySo AbilitySo;

        public readonly Action OnFirePress;
        public readonly Action OnFireRelease;

        protected AbilityState(AbilitySo abilitySo)
        {
            AbilitySo = abilitySo;

            OnFirePress = OnFirePressAction;
            OnFireRelease = OnFireReleaseAction;
        }

        public abstract override void Refresh();

        public abstract override void OnEnter();

        public abstract override void OnExit();

        protected abstract void OnFirePressAction();
        protected abstract void OnFireReleaseAction();
    }
}