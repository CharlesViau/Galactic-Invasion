using Ability;
using Ability.AbilityUI;
using Player;
using UnityEngine;


namespace ScriptableObjects.Ability
{
    [CreateAssetMenu(fileName = "LocationTargeting",
        menuName = "ScriptableObjects/Ability State/Targeting/Location Targeting")]
    public class LocationTargetingState : AbilityTargetingStateSo
    {
        private SpellUI _spellUI;

        public override void Refresh()
        {
            
            TargetingSoClone.Refresh();
            if (_spellUI)
                _spellUI.transform.position = TargetingSoClone.TargetTransform.position;

        }

        public override void OnEnter()
        {
           // Debug.Log("OnEnter: " + AbilitySo.TargetPosition);
            TargetingSoClone.TargetTransform.position = AbilitySo.TargetPosition;

            if (spellUIType != SpellUIType.None)
                _spellUI = SpellUIManager.Instance.Create(spellUIType, new SpellUI.Args(AbilitySo.TargetPosition, AbilitySo.ShootingPosition));
        }

        public override void OnExit()
        {
            if (spellUIType == SpellUIType.None) return;
                SpellUIManager.Instance.Pool(_spellUI);
            
        }

        protected override void OnFirePressEvent()
        {
            AbilitySo.TargetTransform = TargetingSoClone.TargetTransform;
            if (AbilityHandler.Gold >= AbilitySo.stats.goldCost) return;
            MessageUI.Instance.SetText("Not enough money! Costs " + AbilitySo.stats.goldCost);
            MessageUI.Instance.Show();
        }

        protected override void OnFireReleaseEvent()
        {
        }
    }
}