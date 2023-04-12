using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Motherbase;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Motherbase
{
    public class Ring : MonoBehaviour
    {
        [SerializeField] private CoreMotherBase core;

        [SerializeField] int maxHP;
        private int hp;
        public TextMeshProUGUI hpText;

        public AK.Wwise.Event onDamageReceive;

        private void Start()
        {
            hp = maxHP;
        }

        private void ReceiveDamage(int dmg)
        {
            onDamageReceive?.Post(gameObject);
            hp -= dmg;
            core.UpdateColor((float)hp/maxHP);
            hpText.text = ((int)((float)hp/maxHP * 100)).ToString();
        
            if (hp <= 0)
            {
                //Destruction animation
                hpText.text = "0";
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

