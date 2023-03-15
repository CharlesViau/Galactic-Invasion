using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Unity.VisualScripting;
using UnityEngine;

namespace Motherbase
{
    public class CoreMotherBase : MonoBehaviour
    {
        [SerializeField] private List<Shield> shields;
        [SerializeField] private List<ShieldPreview> shields_preview;
        [SerializeField] private int hp;
        
        //TODO replace this with actual shield reference
        private List<int> spawnedShields;

        private void Start()
        {
            spawnedShields = new List<int>();

            for (int i = 0; i < shields_preview.Count; i++)
            {
                shields_preview[i].SetIndex(i);
            }
        }
        private void ReceiveDamage(int dmg)
        {
            hp -= dmg;
    
            if (hp <= 0)
            {
                Debug.Log("Game over");
                //Game over
            }
        }

        public void showShieldsPreview(bool show)
        {
            for (int i = 0; i < shields_preview.Count; i++)
            {
                if (!spawnedShields.Contains(i))
                {
                    shields_preview[i].gameObject.SetActive(show);
                }
            }
        }

        public void spawnShield(int index)
        {
            shields[index].gameObject.SetActive(true);
            spawnedShields.Add(index);
            showShieldsPreview(false);
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

