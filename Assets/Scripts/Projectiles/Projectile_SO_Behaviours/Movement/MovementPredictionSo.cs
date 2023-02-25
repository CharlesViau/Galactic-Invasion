using UnityEngine;

namespace Projectiles.Projectile_SO_Behaviours.Movement
{
    [CreateAssetMenu(fileName = "Movement", menuName = "ScriptableObjects/Projectiles/MovementBehaviour/Prediction")]

    public class MovementPredictionSo : MovementSo
    {

        public override void Init(GameObject owner, Transform target, float speed, Vector3 projectileInitialDirection)
        {
            base.Init(owner, target, speed, projectileInitialDirection);
            owner.transform.forward = projectileInitialDirection.normalized;
            Rb.velocity = projectileInitialDirection;
        }
    }
}