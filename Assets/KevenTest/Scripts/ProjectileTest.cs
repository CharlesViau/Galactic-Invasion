using Enemies;
using Interfaces;
using UnityEngine;

namespace KevenTest.Scripts
{
    public class Projectile : MonoBehaviour
    {
        private float projectileSpeed;
        private int projectileDamage;
        private Vector3 direction;

        public void OnInstantiate(float speed, int damage, Vector3 target)
        {
            projectileSpeed = speed;
            projectileDamage = damage;
            direction = (target - transform.position).normalized;
        }

        private void FixedUpdate()
        {
            transform.position += direction * projectileSpeed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.gameObject.GetComponent<IHittable>().TakeDamage(projectileDamage);
            }
        }
    }
}
