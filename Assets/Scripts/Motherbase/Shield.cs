using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Motherbase 
{
    public class Shield : MonoBehaviour
    {
        [SerializeField] private int hp;
        
        private void ReceiveDamage(int dmg)
        {
            hp -= dmg;
        
            if (hp <= 0)
            {
                //Destruction animation
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

