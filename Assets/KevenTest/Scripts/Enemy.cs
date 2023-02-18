using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Enemy : MonoBehaviour
{
    private Vector3 baseGravityCenter;
    private Rigidbody rb;

    [SerializeField] private Vector3 rotationSpeed;
    [SerializeField] private float gravityStrength = 3f;
    
    public int damage;
    public IGravity gravity;

    void Awake()
    {
        baseGravityCenter = GameObject.Find("MotherBase").transform.position;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = rotationSpeed;
    }

    private void FixedUpdate()
    {
        if (gravity != null)
        {
            gravity.Affect(transform);
        }
        else
        {
            InGravity();
        }
    }

    public void ChangeGravity(IGravity newGravity)
    {
        gravity = newGravity;
        rb.velocity = Vector3.zero;
    }

    private void InGravity()
    {
        Vector3 direction = (baseGravityCenter - transform.position).normalized;
        Vector3 gravityForce = direction * gravityStrength;
        rb.AddForce(gravityForce);
    }
}
