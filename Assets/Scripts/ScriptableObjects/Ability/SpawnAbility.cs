using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Ability
{
    [CreateAssetMenu(fileName = "SpawnAbility", menuName = "ScriptableObjects/Abilities")]
    // ReSharper disable once RequiredBaseTypesIsNotInherited
    public class SpawnAbility : AbilitySo
    {
        [SerializeField] private GameObject abilityPrefab;
        [SerializeField] private float zOffest;

        protected override void ReadyStateRefresh()
        {
        }

        protected override void OnCast()
        {
            var obj = Instantiate(abilityPrefab,
                new Vector3(Owner.TargetPosition.x, Owner.TargetPosition.y, Owner.TargetPosition.z - zOffest),
                Quaternion.Euler(90, 0, 0));
            MessageUI.Instance.Show("Cost: " + stats.goldCost);
        }

        protected override void OnActiveCast()
        {
        }

        protected override void ActiveStateRefresh()
        {
        }
    }
}