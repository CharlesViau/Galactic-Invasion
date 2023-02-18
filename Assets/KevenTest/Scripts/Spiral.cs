using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiral : MonoBehaviour, IGravity
{
    [SerializeField] private float gravityForce;
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

    public void Affect(Transform enemyTransform)
    {
        Vector3 direction = centerPosition - enemyTransform.position;
        direction = Quaternion.Euler(0, 0, 60) * direction;
        float distanceThisFrame = gravityForce * Time.deltaTime;
    
        enemyTransform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }
}
