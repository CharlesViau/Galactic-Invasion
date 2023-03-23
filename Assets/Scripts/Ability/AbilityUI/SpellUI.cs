using System;
using Core;
using UnityEngine;

namespace Ability.AbilityUI
{
    public class SpellUI : MonoBehaviour, IPoolable, ICreatable<SpellUI.Args>
    {
        [SerializeField] private SpellUIType spellUIType;
        public class Args : ConstructionArgs
        {
            public Args(Vector3 spawningPosition) : base(spawningPosition)
            {
            }
        }

        public ValueType ValueType => spellUIType;
        public void Pool()
        {
            
        }

        public void Depool()
        {
           
        }

        public void Construct(Args constructionArgs)
        {
            
        }
    }
}
