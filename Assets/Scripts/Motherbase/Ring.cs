using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Motherbase;
using TMPro;
using UnityEngine;

namespace Motherbase
{
    public class Ring : MonoBehaviour
    {
        [SerializeField] private CoreMotherBase core;

        [SerializeField] int maxHP;
        private int hp;
        public TextMeshProUGUI hpText;

        private void Start()
        {
            hp = maxHP;
        }

        private void ReceiveDamage(int dmg)
        {
            hp -= dmg;
            core.UpdateColor((float)hp/maxHP);
            hpText.text = ((int)((float)hp/maxHP * 100)).ToString();
        
            if (hp <= 0)
            {
                //Destruction animation
                PlayerScore.Instance.stopScore = true;
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

