using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Unity.VisualScripting;
using UnityEngine;

namespace Planets
{
    public class MotherBase : MonoBehaviour
    {
        [SerializeField] private List<GameObject> shields;
        [SerializeField] private int hp;
        
        //TODO replace this with actual shield reference
        private int nextShield = 0;

        private void ReceiveDamage(int dmg)
        {
            hp -= dmg;
    
            if (hp <= 0)
            {
                Debug.Log("Game over");
                //Game over
            }
        }

        public void spawnShield(Vector3 position)
        {
            if (nextShield < shields.Count)
            {
                shields[nextShield].SetActive(true);
                nextShield++;
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

