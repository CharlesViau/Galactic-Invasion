using System;
using Core;
using UnityEngine;

namespace Projectiles.Projectile_SO_Behaviours.Collision
{
    public abstract class OnCollisionSo : ScriptableObject
    {
        public GameObject Owner { get; private set; }
        public int Damage { get; private set; }
        public bool IsPlayerOwner { get; private set; }

        public virtual void Init(GameObject owner, int damage, bool isPlayer)
        {
            Owner = owner;
            Damage = damage;
            IsPlayerOwner = isPlayer;
        }

        public virtual void OnEnterCollision(Vector3 position,
            GameObject collisionObject, bool isPlayer)
        {
        }
    }
}