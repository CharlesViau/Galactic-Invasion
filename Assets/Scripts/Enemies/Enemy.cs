using System;
using Core;
using Interfaces;
using Planets;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Enemies
{
    public class Enemy : MonoBehaviour, ICreatable<Enemy.Args>, IPoolable, IHittable
    {
        public int damage;

        [SerializeField] private Vector3 rotationSpeed;
        [SerializeField] private float gravityStrength = 3f;
        [SerializeField] private int hp;

        private Vector3 baseGravityCenter;
        private Rigidbody rb;
        private bool isAffected;
        private Gravity gravity;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            baseGravityCenter = GameObject.Find("MotherBase").transform.position;
        }

        void Start()
        {
            rb.angularVelocity = rotationSpeed;
            isAffected = false;
        }

        private void FixedUpdate()
        {
            if (gravity != null)
            {
                gravity.Affect(transform);
            }
            else
            {
                if (isAffected)
                {
                    rb.velocity = Vector3.zero;
                    isAffected = false;
                }

                InGravity();
            }
        }

        private void InGravity()
        {
            Vector3 direction = (baseGravityCenter - transform.position).normalized;
            Vector3 gravityForce = direction * gravityStrength;
            rb.AddForce(gravityForce);
        }

        public void ChangeGravity(Gravity newGravity)
        {
            gravity = newGravity;
            rb.velocity = Vector3.zero;
            isAffected = true;
        }

        public void TakeDamage(int _damage)
        {
            hp -= _damage;
            if (hp <= 0)
            {
                EnemyManager.Instance.Pool(this);
            }
        }

        public class Args : ConstructionArgs
        {
            public Quaternion? spawningRotation;

            public Args(Vector3 spawningPosition, Quaternion? spawningRotation = null) : base(spawningPosition)
            {
                this.spawningRotation = spawningRotation;
                this.spawningRotation = spawningRotation;
            }
        }

        public void Construct(Args constructionArgs)
        {
            transform.position = constructionArgs.spawningPosition;

            if (constructionArgs.spawningRotation != null)
                transform.rotation = (Quaternion)constructionArgs.spawningRotation;
        }

        public ValueType ValueType => _type;
        [SerializeField] private EnemyTypes _type;

        public void Pool()
        {
            gameObject.SetActive(false);
        }

        public void Depool()
        {
            gameObject.SetActive(true);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}