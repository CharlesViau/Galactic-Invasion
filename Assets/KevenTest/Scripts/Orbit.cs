using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] private float orbitSpeed;
    [SerializeField] private float timeUntilDestroy;

    private void Update()
    {
        if (timeUntilDestroy <= 0)
        {
            //Play destruction animation
            //Spawn 10 enemies
            Destroy(gameObject);
        }
        timeUntilDestroy -= Time.deltaTime;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            collider.gameObject.GetComponent<Enemy>().EnterOrbit(transform.position, timeUntilDestroy, orbitSpeed);
        }
    }
}
