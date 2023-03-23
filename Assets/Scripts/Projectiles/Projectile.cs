using System;
using Core;
using Projectiles.Projectile_SO_Behaviours.Collision;
using Projectiles.Projectile_SO_Behaviours.Movement;
using UnityEngine;
using UnityEngine.Serialization;


namespace Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour, ICreatable<Projectile.Args>, IPoolable
    {
        [SerializeField] private ProjectileTypes type;
        public ValueType ValueType => type;

        [SerializeField] private MovementSo movementSo;
        [SerializeField] private OnCollisionSo onCollisionSo;
        [SerializeField] private float timeBeforePool;

        private float _timer;
        private bool _isPlayerOwner;

        private void Awake()
        {
            _timer = 0;
            movementSo = Instantiate(movementSo);
            onCollisionSo = Instantiate(onCollisionSo);
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > timeBeforePool)
            {
                ProjectileManager.Instance.Pool(this);
            }

            movementSo.Refresh();
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Enemy"))
            {
                onCollisionSo.OnEnterCollision(collider.ClosestPoint(transform.position),
                                collider.gameObject, _isPlayerOwner);
                ProjectileManager.Instance.Pool(this);
            }
        }

        public void Pool()
        {
            gameObject.SetActive(false);
        }

        // ReSharper disable once IdentifierTypo
        public void Depool()
        {
            _timer = 0;
            gameObject.SetActive(true);
            movementSo.Rb.velocity = Vector3.zero;
            movementSo.Rb.angularVelocity = Vector3.zero;
        }

        public void Construct(Args constructionArgs)
        {
            _isPlayerOwner = constructionArgs.IsPlayerOwner;
            transform.position = constructionArgs.spawningPosition;
            movementSo.Init(gameObject, constructionArgs.Target, constructionArgs.BulletSpeed,
                constructionArgs.VelocityDirection);
            // ReSharper disable once Unity.InefficientPropertyAccess
            onCollisionSo.Init(gameObject, constructionArgs.BulletDamage, constructionArgs.IsPlayerOwner);
        }

        public class Args : ConstructionArgs
        {
            public float BulletSpeed { get; }
            public Transform Target { get; }
            public int BulletDamage { get; }
            public Vector3 VelocityDirection { get; }
            public bool IsPlayerOwner { get; }

            public Args(Vector3 spawningPosition, Transform target, float bulletSpeed, int bulletDamage,
                Vector3 velocityDirection, bool isPlayerOwner) : base(spawningPosition)
            {
                BulletSpeed = bulletSpeed;
                Target = target;
                BulletDamage = bulletDamage;
                VelocityDirection = velocityDirection;
                IsPlayerOwner = isPlayerOwner;
            }
        }
    }
}