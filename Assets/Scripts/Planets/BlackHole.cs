using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Planets
{
    public class BlackHole : MonoBehaviour
    {
        [SerializeField] private float timer;

        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
        
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
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

