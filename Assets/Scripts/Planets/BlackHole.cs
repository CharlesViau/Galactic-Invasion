using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Planets
{
    public class BlackHole : MonoBehaviour
    {
        [SerializeField] private int numberOfEnemies;
    
        private void AbsorbEnemy()
        {
            numberOfEnemies -= 1;
            if (numberOfEnemies <= 0)
            {
                //Play animation
                
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
        
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Enemy"))
            {
                AbsorbEnemy();
                EnemyManager.Instance.Pool(collider.gameObject.GetComponent<Enemy>());
            }
        }
    }
}

