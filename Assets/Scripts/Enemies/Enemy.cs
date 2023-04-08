using System;
using Core;
using Interfaces;
using Planets;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour, ICreatable<Enemy.Args>, IPoolable, IHittable
    {
        public int damage;

        [SerializeField] private Vector3 rotationSpeed;
        [SerializeField] private float gravityStrength = 3f;
        [SerializeField] private int maxHP;
        private int hp;

        public int reward;
        [SerializeField] private EnemyTypes _type;

        private Vector3 _baseGravityCenter;
        private Gravity _gravity;
        private bool _isAffected;
        [NonSerialized] private Vector3 _velocity;

        private Rigidbody rb;

        [NonSerialized] public bool isPoolable = true;
        private void Awake()
        {
            _baseGravityCenter = GameObject.Find("MotherBase").transform.position;
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _velocity = Vector3.zero;
            _isAffected = false;
            hp = maxHP;
        }

        private void FixedUpdate()
        {
            transform.Rotate(rotationSpeed * Time.deltaTime);
            if (_gravity != null)
            {
                _gravity.Affect(transform);
            }
            else
            {
                if (_isAffected)
                {
                    _velocity = Vector3.zero;
                    _isAffected = false;
                }

                InGravity();
            }
        }

        public void Construct(Args constructionArgs)
        {
            transform.position = constructionArgs.spawningPosition;

            if (constructionArgs.SpawningRotation != null)
                transform.rotation = (Quaternion)constructionArgs.SpawningRotation;
        }

        public void TakeDamage(int _damage)
        {
            hp -= _damage;
            if (hp <= 0) Die();
        }

        public ValueType ValueType => _type;

        public void Pool()
        {
            gameObject.SetActive(false);
        }

        public void Depool()
        {
            gameObject.SetActive(true);
            rb.velocity = Vector3.zero;
            _velocity = Vector3.zero;
            _gravity = null;
            _isAffected = false;
            hp = maxHP;
        }

        private void Die()
        {
            if (!isPoolable)
            {
                Destroy(gameObject);
            }
            else
            {
                EnemyManager.OnEnemyDeathEvent(reward);
                EnemyManager.Instance.Pool(this);
            }
            
        }

        private void InGravity()
        {
            var position = transform.position;
            var direction = (_baseGravityCenter - position).normalized;
            var gravityForce = direction * gravityStrength;

            _velocity = _velocity + gravityForce * Time.deltaTime;

            position = position + _velocity * Time.deltaTime +
                       gravityForce / 2 * Mathf.Pow(Time.deltaTime, 2);
            transform.position = position;
        }

        public void ChangeGravity(Gravity newGravity)
        {
            _gravity = newGravity;
            _isAffected = true;
        }


        public class Args : ConstructionArgs
        {
            public Quaternion? SpawningRotation;

            public Args(Vector3 spawningPosition, Quaternion? spawningRotation = null) : base(spawningPosition)
            {
                SpawningRotation = spawningRotation;
                SpawningRotation = spawningRotation;
            }
        }
    }
}