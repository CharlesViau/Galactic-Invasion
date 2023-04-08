using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Enemies;
using UnityEngine;
using UnityEngine.UI;
using Motherbase;
using Towers;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace Motherbase 
{
    public class Shield : MonoBehaviour
    {
        public delegate void DeathEvent();
        public event DeathEvent deathEvent;

        public bool isSelectable = false;

        [SerializeField] private int maxHP;
        [SerializeField] private Transform t;
        [SerializeField] private Image healthBar;

        private bool isRingDestroyed = false;
        private bool imageDestroyed = false;
        private Vector3 direction;
        private float gravityStrength;
        private Vector3 velocity;
        private int hp;
        private Transform rocket;
        private Transform sniper;
        private Tower tower;
        
        private Color _baseColor;
        private Color _selectedColor;
        private Material _material;
        //private Vector3 rotationSpeed;

        private void Start()
        {
            velocity = Vector3.zero;
            _baseColor = new Color((float)188/255, (float)173/255, (float)173/255, 1f);
            _selectedColor = new Color((float)102/255, (float)212/255, (float)75/255, 1f);
            _material = gameObject.GetComponent<Renderer>().material;
        }

        private void OnEnable()
        {
            healthBar.gameObject.transform.parent.gameObject.SetActive(true);
            hp = maxHP;
            UpdateHealthBar(hp, maxHP);
        }

        public void GetTowerReferences()
        {
            tower = transform.Find("Tower_lvl_one").GetComponent<Tower>();
            rocket = transform.Find("Tower_lvl_one/tourelle01_v03/tourelleRotation_grp/arme_grp/rocket_grp");
            sniper = transform.Find("Tower_lvl_one/tourelle01_v03/tourelleRotation_grp/arme_grp/sniper_grp");
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

        public void Upgrade(int lvl)
        {
            if (lvl == 2)
            {
                rocket.gameObject.SetActive(true);
            } else if (lvl == 3)
            {
                sniper.gameObject.SetActive(true);
            }
            tower.Upgrade(lvl);
        }

        public void Repair()
        {
            hp = maxHP;
            UpdateHealthBar(hp, maxHP);
            _material.color = _baseColor;
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
        
        private void OnMouseEnter()
        {
            if (isSelectable)
                _material.color = _selectedColor;
        }

        private void OnMouseExit()
        {
            if (isSelectable)
                _material.color = _baseColor;
        }
    }
}

