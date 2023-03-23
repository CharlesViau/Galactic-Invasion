using UnityEngine;

namespace Interfaces
{
    public interface ITargetAcquirer
    {
        public abstract Transform ShootingPosition { get; }
        public abstract Vector3 AimingDirection { get; }
        public abstract Vector3 TargetPosition{ get; }
        public abstract Transform TargetTransform { get; set; }
    }
}
