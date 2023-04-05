using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Enemies;
using UnityEngine;
using UnityEngine.UI;
using Motherbase;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace Motherbase 
{
    public class Shield : MonoBehaviour
    {
        public delegate void DeathEvent();
        public event DeathEvent deathEvent;
        
        [SerializeField] private int maxHP;
        [SerializeField] private Transform t;
        private int hp;
        [SerializeField] private Image healthBar;

        private bool isRingDestroyed = false;
        private bool imageDestroyed = false;
        private Vector3 direction;
        private float gravityStrength;
        private Vector3 velocity;
        //private Vector3 rotationSpeed;

        private void Start()
        {
            velocity = Vector3.zero;
            //rotationSpeed = new Vector3(10,0,0);
        }

        private void OnEnable()
        {
            healthBar.gameObject.transform.parent.gameObject.SetActive(true);
            hp = maxHP;
            UpdateHealthBar(hp, maxHP);
        }

        private void FixedUpdate()
        {
            if (isRingDestroyed)
            {
                Vector3 gravityForce = direction * gravityStrength;
                velocity = velocity + (gravityForce * Time.deltaTime);
                transform.position = transform.position + (velocity * Time.deltaTime) + ((gravityForce/2) * (Mathf.Pow(Time.deltaTime, 2 )));
                //transform.Rotate(rotationSpeed, Space.World);
            }
        }

        public void RingDestroyed()
        {
            isRingDestroyed = true;
            direction = (t.position - Vector3.zero).normalized;
            gravityStrength = Random.Range(6f, 8f);
            healthBar.gameObject.transform.parent.gameObject.SetActive(false);
        }
        private void ReceiveDamage(int dmg)
        {
            hp -= dmg;

            if (hp <= 0)
            {
                gameObject.SetActive(false);
                //Destruction animation
                if (deathEvent != null)
                {
                    deathEvent();
                }
                healthBar.gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                UpdateHealthBar(hp, maxHP);
            }
        }
        
        private void UpdateHealthBar(int currentHP, int maxHP)
        {
            float healthPercentage = (float)currentHP / maxHP;
            healthBar.fillAmount = healthPercentage;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                ReceiveDamage(enemy.damage);
                if (!enemy.isPoolable)
                {
                    Destroy(enemy.gameObject);
                }
                else
                {
                    EnemyManager.Instance.Pool(collider.gameObject.GetComponent<Enemy>());
                }
            }
        }
    }
}

