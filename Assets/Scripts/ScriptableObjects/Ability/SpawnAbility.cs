using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Ability
{ 
    [CreateAssetMenu(fileName = "SpawnAbility", menuName = "ScriptableObjects/Abilities")]
    // ReSharper disable once RequiredBaseTypesIsNotInherited
    public class SpawnAbility : AbilitySo
    {
        [SerializeField] private GameObject abilityPrefab;

        protected override void ReadyStateRefresh()
        {
            
        }

        protected override void OnCast()
        {
            Instantiate(abilityPrefab, Owner.TargetTransform.position,TargetTransform.rotation);
            MessageUI.Instance.SetText("Cost: " + stats.goldCost);
            MessageUI.Instance.Show();
        }

        protected override void OnActiveCast()
        {
            
        }

        protected override void ActiveStateRefresh()
        {
            
        }
    }
}

