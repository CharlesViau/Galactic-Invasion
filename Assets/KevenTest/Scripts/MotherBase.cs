using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MotherBase : MonoBehaviour
{
    [SerializeField] private int hp;

    private void ReceiveDamage(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            Debug.Log("Game over");
            //Game over
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Debug.Log("BasePlanet hit");
            ReceiveDamage(collider.gameObject.GetComponent<Enemy>().damage);
            Destroy(collider.gameObject);
        }
    }
}
