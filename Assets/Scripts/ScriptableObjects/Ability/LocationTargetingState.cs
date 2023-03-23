using Ability;
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

            //if (SpellUIType != SpellUIType.None)
                //_spellUI = SpellUIManager.Instance.Create(SpellUIType, new SpellUI.Args(AbilitySo.TargetPosition));
        }

        public override void OnExit()
        {
            if (SpellUIType == SpellUIType.None) return;
            //SpellUIManager.Instance.Pool(SpellUIType, _spellUI);
            
        }

        protected override void OnFirePressEvent()
        {
            AbilitySo.TargetTransform = TargetingSoClone.TargetTransform;
        }

        protected override void OnFireReleaseEvent()
        {
        }
    }
}