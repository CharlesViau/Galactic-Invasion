using System;
using Enemies;
using Interfaces;
using UnityEngine;

namespace Projectiles.Projectile_SO_Behaviours.Collision
{
    [CreateAssetMenu(fileName = "OnCollision", menuName = "ScriptableObjects/OnCollision/HitObject")]
    public class DoSingleTargetDamage : OnCollisionSo
    {
        public override void OnEnterCollision(Vector3 position, GameObject collisionObject, bool isPlayerOwner)
        {
            if (!collisionObject.TryGetComponent<IHittable>(out var hittable)) return;
            if (IsPlayerOwner && collisionObject.CompareTag("Enemy") ||
                !IsPlayerOwner && !collisionObject.CompareTag("Enemy"))
            {
                hittable.TakeDamage(Damage);
            }
        }
    }
}