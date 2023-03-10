using Core;
using UnityEngine;
using UnityEngine.Scripting;

namespace Enemies
{
    public class EnemyManager : MonoBehaviourManager<Enemy,EnemyTypes, Enemy.Args, EnemyManager>
    {
        protected override string PrefabLocation => "Prefabs/Enemies/";
        
        [Preserve]
        public Transform GetClosest(Transform currentPosition, float range)
        {
            Transform transform = null;
            range *= range;
            foreach (var enemy in collection)
            {
                var newDistance = Vector3.SqrMagnitude(currentPosition.position - enemy.transform.position);
                // if in the range and they are alive (not in the pool)
                if (!(newDistance < range) || !enemy.isActiveAndEnabled) continue; 
                range = newDistance;
                transform = enemy.transform;
            }
            return transform;
        }
    }
    
    
}
