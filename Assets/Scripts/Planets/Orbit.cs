using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Enemies;
using UnityEngine;

namespace Planets
{
    public class Orbit : Gravity
    {
        [SerializeField] private float orbitSpeed;
        [SerializeField] private float timeUntilDestroy;

        [SerializeField] private Shards shards;
        
        private Vector3 centerPosition;

        private void Start()
        {
            centerPosition = transform.position;
        }
        private void Update()
        {
            if (timeUntilDestroy <= 0)
            {
                Destruction();
            }
            timeUntilDestroy -= Time.deltaTime;
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
            enemyTransform.RotateAround(centerPosition, Vector3.forward, orbitSpeed * Time.deltaTime);
        }

        private void Destruction()
        {
            shards.ActivateShards();
            Destroy(gameObject);
        }
    }
}

