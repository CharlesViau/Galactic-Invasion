using ScriptableObjects.Ability;
using UnityEngine;

namespace Abilities.TargetingSO
{
    [CreateAssetMenu(fileName = "RaycastTargeting", menuName = "ScriptableObjects/TargetingSo/PlayerRaycastTargeting")]
    public class RaycastPointTargeting : TargetingSo
    {
        public override void Refresh()
        {
            TemporaryTransform.position = Owner.TargetPosition;
        }
    }
}