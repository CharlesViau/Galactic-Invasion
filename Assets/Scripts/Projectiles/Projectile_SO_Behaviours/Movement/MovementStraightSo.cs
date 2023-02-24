using UnityEngine;

namespace Projectiles.Projectile_SO_Behaviours.Movement
{
    [CreateAssetMenu(fileName = "StraightMovement", menuName = "ScriptableObjects/Projectiles/Movement/Straight")]

    public class MovementStraightSo : MovementSo
    {
        public override void Init(GameObject owner,Transform targetTransform, float speed,Vector3 projectileInitialDirection)
        {
            base.Init(owner, targetTransform, speed, projectileInitialDirection);
            var position = targetTransform.position;
            var position1 = owner.transform.position;
            owner.transform.forward = (position - position1).normalized;
            Rb.velocity = (position - position1).normalized * InitialSpeed;
        }
    }
}