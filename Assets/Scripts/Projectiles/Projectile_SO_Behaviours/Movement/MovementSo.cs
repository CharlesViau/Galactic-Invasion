using UnityEngine;

namespace Projectiles.Projectile_SO_Behaviours.Movement
{
    public abstract class MovementSo : ScriptableObject
    {
        public GameObject Owner { get; private set; }
        public Transform Target { get; private set; }
        public Rigidbody Rb { get; private set; }
        public Vector3 ProjectileInitialDirection { get; private set; }
        public float InitialSpeed { get; private set; }

        public virtual void Init(GameObject owner, Transform target, float speed,
            Vector3 projectileInitialDirection)
        {
            Owner = owner;
            Target = target;
            Rb = owner.GetComponent<Rigidbody>();
            InitialSpeed = speed;
            ProjectileInitialDirection = projectileInitialDirection;
        }

        public virtual void Refresh()
        {
        }

        public virtual void FixedRefresh()
        {
        }

    }
}