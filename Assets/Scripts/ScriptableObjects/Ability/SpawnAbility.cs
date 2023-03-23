using UnityEngine;

namespace ScriptableObjects.Ability
{ 
    [CreateAssetMenu(fileName = "SpawnAbility", menuName = "ScriptableObjects/Abilities")]
    // ReSharper disable once RequiredBaseTypesIsNotInherited
    public class SpawnAbility : AbilitySo
    {
        //public AbilityType Type;

        protected override void ReadyStateRefresh()
        {
            
        }

        protected override void OnCast()
        {
            //AbilityManager.Instance.Create(AbilityType, new Ability.Args(TargetTransform.position));
        }

        protected override void OnActiveCast()
        {
            
        }

        protected override void ActiveStateRefresh()
        {
            
        }
    }
}

