using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform gravityCenter;

    [SerializeField] private Vector3 rotationSpeed;
    [SerializeField] private float gravityStrength = 3f;
    [SerializeField] private int damage;
    
    private Rigidbody rb;

    void Awake()
    {
        gravityCenter = GameObject.Find("BasePlanet").transform;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = rotationSpeed;
    }

    private void FixedUpdate()
    {
        Vector3 direction = (gravityCenter.position - transform.position).normalized;
        Vector3 gravityForce = direction * gravityStrength;
        rb.AddForce(gravityForce);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Orbit")
        {
            Debug.Log("changing orbit");
            gravityCenter = collider.gameObject.transform.parent.transform;
            gravityStrength = collider.gameObject.transform.parent.GetComponent<PlanetGravity>().gravityForce;
        } else if (collider.tag == "Planet")
        {
            Debug.Log("Planet hit");
            collider.gameObject.GetComponent<PlanetGravity>().ReceiveDamage(damage);
            Destroy(gameObject);
        } else if (collider.tag == "Base Planet")
        {
            Debug.Log("BasePlanet hit");
            Destroy(gameObject);
        }
    }
}
