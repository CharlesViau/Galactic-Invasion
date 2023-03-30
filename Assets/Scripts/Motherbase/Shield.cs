using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Enemies;
using UnityEngine;
using UnityEngine.UI;
using Motherbase;
using Vector3 = UnityEngine.Vector3;

namespace Motherbase 
{
    public class Shield : MonoBehaviour
    {
        [SerializeField] private int hp;
        [SerializeField] private Transform t;
        private int maxHP;
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
            maxHP = hp;
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
            if (!imageDestroyed)
            {
                Destroy(healthBar.gameObject);
            }
        }
        private void ReceiveDamage(int dmg)
        {
            hp -= dmg;

            if (hp <= 0)
            {
                //Destruction animation
                gameObject.SetActive(false);
                if (!imageDestroyed)
                {
                    Destroy(healthBar.gameObject);
                }
                imageDestroyed = true;
            }
            else
            {
                if (!isRingDestroyed)
                {
                    UpdateHealthBar(hp, maxHP);
                }
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

