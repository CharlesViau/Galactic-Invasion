using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Motherbase;
using UnityEngine;

namespace Motherbase
{
    public class Ring : MonoBehaviour
    {
        [SerializeField] private CoreMotherBase core;

        [SerializeField] int maxHP;
        private int hp;

        private void Start()
        {
            hp = maxHP;
        }

        private void ReceiveDamage(int dmg)
        {
            hp -= dmg;
            core.UpdateColor((float)hp/maxHP);
        
            if (hp <= 0)
            {
                //Destruction animation
                core.DestroyShields();
                gameObject.SetActive(false);
            }
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

