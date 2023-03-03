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
        [NonSerialized] public Vector3 velocity;

        [SerializeField] private Vector3 rotationSpeed;
        [SerializeField] private float gravityStrength = 3f;
        [SerializeField] private int hp;

        private Vector3 baseGravityCenter;
        private bool isAffected;
        private Gravity gravity;


        void Awake()
        {
            baseGravityCenter = GameObject.Find("MotherBase").transform.position;
        }

        void Start()
        {
            velocity = Vector3.zero;
            isAffected = false;
        }

        private void FixedUpdate()
        {
            transform.Rotate(rotationSpeed * Time.deltaTime);
            if (gravity != null)
            {
                gravity.Affect(transform);
            }
            else
            {
                if (isAffected)
                {
                    velocity = Vector3.zero;
                    isAffected = false;
                }

                InGravity();
            }
        }

        private void InGravity()
        {
            Vector3 direction = (baseGravityCenter - transform.position).normalized;
            Vector3 gravityForce = direction * gravityStrength;
            velocity = velocity + (gravityForce * Time.deltaTime);
            transform.position = transform.position + (velocity * Time.deltaTime) + ((gravityForce/2) * (Mathf.Pow(Time.deltaTime, 2 )));
        }

        public void ChangeGravity(Gravity newGravity)
        {
            gravity = newGravity;
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
            velocity = Vector3.zero;
            gravity = null;
            isAffected = false;
        }
    }
}