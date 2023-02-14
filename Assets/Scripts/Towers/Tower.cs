using System;
using Core;
using UnityEngine;

namespace Towers
{
    public class Tower : MonoBehaviour, ICreatable<Tower.Args>
    {
        #region ICreatable Implementation
        public void Construct(Args constructionArgs)
        {
            transform.position = constructionArgs.spawningPosition;
        }
        public class Args : ConstructionArgs
        {

            public Args(Vector3 spawningPosition) : base(spawningPosition)
            {

            }

        }
        
        #endregion
        
        #region State Machine

        private class TowerStateMachine
        {
            
        }
        #endregion
    }
}
