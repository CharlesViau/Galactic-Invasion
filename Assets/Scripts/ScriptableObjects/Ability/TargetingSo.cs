using Interfaces;
using UnityEngine;

namespace ScriptableObjects.Ability
{
    public abstract class TargetingSo : ScriptableObject
    {
        protected ITargetAcquirer Owner;
        protected Transform TemporaryTransform;
        

        public Transform TargetTransform => TemporaryTransform;
        
        public virtual void Init(ITargetAcquirer owner)
        {
            Owner = owner;
            TemporaryTransform = new GameObject("tempTransform_targetingSO").transform;
        }

        public abstract void Refresh();
    }
}