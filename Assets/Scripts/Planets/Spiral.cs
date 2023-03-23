using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Planets
{
    public class Spiral : Gravity
    {
        [SerializeField] private float gravityForce;
        [SerializeField] private float spiralForce;
        private Vector3 centerPosition;

        private void Start()
        {
            centerPosition = transform.position;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.gameObject.GetComponent<Enemy>().ChangeGravity(this);
            }
        }
    
        public override void Affect(Transform enemyTransform)
        {
            Vector3 direction = centerPosition - enemyTransform.position;
            direction = Quaternion.Euler(0, 0, spiralForce) * direction;
            float distanceThisFrame = gravityForce * Time.deltaTime;
        
            enemyTransform.Translate(direction.normalized * distanceThisFrame, Space.World);
        }
    }
}

