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

        [SerializeField] int hp;
        
        private void ReceiveDamage(int dmg)
        {
            hp -= dmg;
        
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
                ReceiveDamage(collider.gameObject.GetComponent<Enemy>().damage);
                EnemyManager.Instance.Pool(collider.gameObject.GetComponent<Enemy>());
            }
        }
    }
}

