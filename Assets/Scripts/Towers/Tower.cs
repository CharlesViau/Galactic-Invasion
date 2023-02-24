using System;
using Core;
using UnityEngine;

namespace Towers
{
    public class Tower : MonoBehaviour, ICreatable<Tower.Args>
    {
        #region Properties and Variables
        
        #endregion
        
        #region Events
        public event Action OnFire;
        
        #endregion

        #region ICreatable Implementation

        public void Construct(Args constructionArgs)
        {
            transform.position = constructionArgs.spawningPosition;
            if (constructionArgs.spawningRotation != null)
                transform.rotation = (Quaternion)constructionArgs.spawningRotation;
        }

        public class Args : ConstructionArgs
        {
            public Quaternion? spawningRotation;

            public Args(Vector3 spawningPosition, Quaternion? spawningRotation = null) : base(spawningPosition)
            {
                if (spawningRotation != null)
                    this.spawningRotation = spawningRotation;
            }
        }

        #endregion

        public void Fire()
        {
            OnFire?.Invoke();
        }
    }
}