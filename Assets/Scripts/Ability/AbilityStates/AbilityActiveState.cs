using System;
using ScriptableObjects.Ability;
using UnityEngine;

namespace Ability.AbilityStates
{
    public class AbilityActiveState : AbilityState
    {
        
        private readonly Action _onActiveCast;
        private readonly Action _onCast;
        private readonly Action _activeStateRefresh;

        private float _activeTimeRemaining;
        private int _recastChargesRemaining;

        public bool HasNoRecastChargesLeft => _recastChargesRemaining == 0;
        public bool ActiveTimeRemainingIsOver => _activeTimeRemaining <= 0;
        public bool HasActiveTimer => AbilitySo.activeTime > 0;

        public AbilityActiveState(AbilitySo abilitySo,Action onCast, Action onActiveCast, Action activeStateRefresh) : base(abilitySo)
        {
            _onCast = onCast;
            _onActiveCast = onActiveCast;
            _activeStateRefresh = activeStateRefresh;
            _activeTimeRemaining = abilitySo.activeTime;
            _recastChargesRemaining = abilitySo.recastCharges;
            
        }
        public override void Refresh()
        {
            if (_activeTimeRemaining > 0)
            {
                _activeTimeRemaining -= Time.deltaTime;
            }
            
            _activeStateRefresh.Invoke();
        }

        public override void OnEnter()
        {
            _recastChargesRemaining = AbilitySo.recastCharges;
            _activeTimeRemaining = AbilitySo.activeTime;
            _onCast.Invoke();
        }

        public override void OnExit()
        {
           
        }

        protected override void OnFirePressAction()
        {
           _onActiveCast.Invoke();
        }

        protected override void OnFireReleaseAction()
        {
            
        }
    }
}