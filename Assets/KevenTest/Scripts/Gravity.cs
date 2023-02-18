using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField] private float gravityForce;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            //collider.gameObject.GetComponent<Enemy>().UpdateGravityCenter(transform.position, gravityForce);
        }
    }
}
