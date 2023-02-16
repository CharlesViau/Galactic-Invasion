using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform gravityCenter;
    private Transform baseGravityCenter;
    private bool isInOrbit;
    private float orbitTimer;
    private Vector3 orbitCenterPoint;
    private float orbitSpeed;

    [SerializeField] private Vector3 rotationSpeed;
    [SerializeField] private float gravityStrength = 3f;
    
    public int damage;

    private Rigidbody rb;

    void Awake()
    {
        baseGravityCenter = GameObject.Find("MotherBase").transform;
        gravityCenter = baseGravityCenter;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = rotationSpeed;
    }

    private void FixedUpdate()
    {
        if (isInOrbit)
        {
            InOrbit();
        }
        else
        {
            InGravity();
        }
    }

    public void UpdateGravityCenter(Transform newGravityCenter, float newGravityStrength)
    {
        gravityCenter = newGravityCenter;
        gravityStrength = newGravityStrength;
    }

    public void EnterOrbit(Vector3 position, float timer, float speed)
    {
        isInOrbit = true;
        orbitTimer = timer;
        orbitCenterPoint = position;
        orbitSpeed = speed;
        rb.velocity = Vector3.zero;
    }

    private void InOrbit()
    {
        orbitTimer -= Time.deltaTime;
        if (orbitTimer <= 0)
        {
            gravityCenter = baseGravityCenter;
            isInOrbit = false;
        }
        else
        {
            transform.RotateAround(orbitCenterPoint, Vector3.forward, orbitSpeed * Time.deltaTime);
        }
    }

    private void InGravity()
    {
        if (!gravityCenter)
        {
            gravityCenter = baseGravityCenter;
        }
        Vector3 direction = (gravityCenter.position - transform.position).normalized;
        Vector3 gravityForce = direction * gravityStrength;
        rb.AddForce(gravityForce);
    }
}
