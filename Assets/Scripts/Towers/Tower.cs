using System;
using Core;
using Enemies;
using Projectiles;
using UnityEngine;
using Projectile = Projectiles.Projectile;

namespace Towers
{
    [RequireComponent(typeof(Animator))]
    public class Tower : MonoBehaviour, ICreatable<Tower.Args>, IPoolable
    {
        #region Properties and Variables

        [Header("Tower Stats")] [SerializeField]
        private int detectionRange;

        public int DetectionRange => detectionRange;

        [SerializeField] private float attackSpeed;
        public float AttackSpeed => attackSpeed;

        [SerializeField] private TowerTypes types;
        public ValueType ValueType => types;

        [Header("Things to Instantiate")] [SerializeField]
        private ProjectileTypes projectileType;

        //[SerializeField] private ParticleType particleType;

        [Header("Projectile Stats")] [SerializeField]
        private int projectileDamage;

        public int ProjectileDamage => projectileDamage;

        [SerializeField] private float projectileSpeed;

        public float ProjectileSpeed => projectileSpeed;

        [Header("Tower Parts and Positions")] [SerializeField]
        private Transform head;

        [SerializeField] private Transform barrel;
        [SerializeField] private Transform particlePosition;

        //Animation Stuff
        private static readonly int Fire1 = Animator.StringToHash("Fire");
        private Animator _animator;

        //Other
        private Vector3 _projectileVelocity = Vector3.zero;
        private Transform _target;
        private float _timer = 0;
        private int _detectionRangePow2;
        private Rigidbody _targetRb;

        #endregion

        #region Events

        public event Action<Transform> OnFire;

        #endregion

        #region ICreatable Implementation

        public void Construct(Args constructionArgs)
        {
            transform.position = constructionArgs.spawningPosition;
            if (constructionArgs.SpawningRotation != null)
                transform.rotation = (Quaternion)constructionArgs.SpawningRotation;
        }

        public class Args : ConstructionArgs
        {
            public Quaternion? SpawningRotation { get; }

            public Args(Vector3 spawningPosition, Quaternion? spawningRotation = null) : base(spawningPosition)
            {
                if (spawningRotation != null)
                    SpawningRotation = spawningRotation;
            }
        }

        #endregion

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _detectionRangePow2 = detectionRange * detectionRange;

            OnFire += AimAndFire;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_target is null || !_target.gameObject.activeSelf || IsTargetOutOfRange())
            {
                _target = Helper.Helper.GetClosetInRange(typeof(EnemyManager), transform, detectionRange);
                if (_target)
                    _targetRb = _target.GetComponent<Rigidbody>();
            }
            
            if (_timer > attackSpeed && _target)
            {
                OnFire?.Invoke(_target);
                _timer = 0;
            }
        }

        private void AimAndFire(Transform target)
        {
            var position = head.position;
            var position1 = target.position;
            head.forward = (position1 - position).normalized;
            var targetMovementDirection = _targetRb.velocity;
            var targetMovementSpeed = targetMovementDirection.magnitude;
            var angleTargetToPlayer = Vector3.Angle(targetMovementDirection.normalized,
                (position - position1).normalized);
            var towerAngleFinalRotation =
                Mathf.Asin((Mathf.Sin(angleTargetToPlayer * Mathf.Deg2Rad) * targetMovementSpeed) /
                           projectileSpeed) * Mathf.Rad2Deg;

            var dir = (position1 - position).normalized;
            var left = Vector3.Cross(dir, targetMovementDirection.normalized);
            head.LookAt(position1, left);
            if (!float.IsNaN(towerAngleFinalRotation))
            {
                var transform1 = head.transform;
                head.RotateAround(transform1.position, transform1.up, towerAngleFinalRotation);
            }

            _projectileVelocity = head.forward * projectileSpeed;
            ProjectileManager.Instance.Create(projectileType,
                new Projectile.Args(head.position, target, projectileSpeed, projectileDamage,
                    _projectileVelocity, true));

            //_animator.SetTrigger(Fire1);

            //ParticleSystemManager.Instance.Create(particleType,
            //new ParticleSystemScript.Args(particlePosition.position));
        }

        private bool IsTargetOutOfRange()
        {
            return Vector3.SqrMagnitude(transform.position - _target.transform.position) > _detectionRangePow2;
        }

        public void Pool()
        {
            gameObject.SetActive(false);
        }

        public void Depool()
        {
            gameObject.SetActive(true);
        }
    }
}