using System;
using Core;
using Enemies;
using Projectiles;
using UnityEngine;

namespace Towers
{
    [RequireComponent(typeof(Animator))]
    public class Tower : MonoBehaviour, ICreatable<Tower.Args>, IPoolable
    {
        private void Awake()
        {
            _privateDetectionRange = detectionRange;
            _privateAttackSpeed = attackSpeed;
            _animator = GetComponent<Animator>();
            _detectionRangePow2 = _privateDetectionRange * _privateDetectionRange;

            _headOgPosition = head.position;

            OnFire += AimAndFire;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            /*if (_target == null || _target is null || !_target.gameObject.activeSelf || IsTargetOutOfRange())
            {*/
            _target = EnemyManager.Instance.GetClosest(transform, _privateDetectionRange);
            if (_target)
                _targetRb = _target.GetComponent<Rigidbody>();
            //}

            if (_timer > _privateAttackSpeed && _target && IsTargetInView())
            {
                OnFire?.Invoke(_target);
                _timer = 0;
            }
        }

        public void Pool()
        {
            gameObject.SetActive(false);
        }

        public void Depool()
        {
            gameObject.SetActive(true);
        }

        public void ResetTower()
        {
            projectileType = ProjectileTypes.GreenLaser;
            _privateDetectionRange = detectionRange;
            _privateAttackSpeed = attackSpeed;
        }

        #region Events

        public event Action<Transform> OnFire;

        #endregion

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
                Mathf.Asin(Mathf.Sin(angleTargetToPlayer * Mathf.Deg2Rad) * targetMovementSpeed /
                           projectileSpeed) * Mathf.Rad2Deg;

            var dir = (position1 - position).normalized;
            var left = Vector3.Cross(dir, targetMovementDirection.normalized);
            barrel.LookAt(position1, left);
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

        private bool IsTargetInView()
        {
            RaycastHit hit;
            if (Physics.Raycast(_headOgPosition, _target.position - _headOgPosition, out hit))
                if (hit.transform == _target)
                    return true;
            return false;
        }

        private bool IsTargetOutOfRange()
        {
            return Vector3.SqrMagnitude(transform.position - _target.transform.position) > _detectionRangePow2;
        }

        public void Upgrade(int lvl)
        {
            if (lvl == 2)
                projectileType = ProjectileTypes.PinkLaser;
            else if (lvl == 3) projectileType = ProjectileTypes.BlueLaser;
            _privateDetectionRange = (int)(_privateDetectionRange * 1.2);
            _privateAttackSpeed = (float)(_privateAttackSpeed * 0.8);
        }

        #region Properties and Variables

        [Header("Tower Stats")] [SerializeField]
        private int detectionRange;

        private int _privateDetectionRange;
        private float _privateAttackSpeed;

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

        private Vector3 _headOgPosition;

        [SerializeField] private Transform barrel;
        [SerializeField] private Transform particlePosition;

        //Animation Stuff
        private static readonly int Fire1 = Animator.StringToHash("Fire");
        private Animator _animator;

        //Other
        private Vector3 _projectileVelocity = Vector3.zero;
        private Transform _target;
        private float _timer;
        private int _detectionRangePow2;
        private Rigidbody _targetRb;

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
            public Args(Vector3 spawningPosition, Quaternion? spawningRotation = null) : base(spawningPosition)
            {
                if (spawningRotation != null)
                    SpawningRotation = spawningRotation;
            }

            public Quaternion? SpawningRotation { get; }
        }

        #endregion
    }
}