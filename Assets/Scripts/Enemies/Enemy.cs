using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Planets;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Enemy : MonoBehaviour
{
    private Vector3 baseGravityCenter;
    private Rigidbody rb;
    private bool isAffected;

    [SerializeField] private Vector3 rotationSpeed;
    [SerializeField] private float gravityStrength = 3f;
    
    public int damage;
    private Gravity gravity;

    void Awake()
    {
        baseGravityCenter = GameObject.Find("MotherBase").transform.position;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = rotationSpeed;
        isAffected = false;
    }

    private void FixedUpdate()
    {
        if (gravity != null)
        {
            gravity.Affect(transform);
        }
        else
        {
            if (isAffected)
            {
                rb.velocity = Vector3.zero;
                isAffected = false;
            }
            InGravity();
        }
    }

    public void ChangeGravity(Gravity newGravity)
    {
        gravity = newGravity;
        rb.velocity = Vector3.zero;
        isAffected = true;
    }

    private void InGravity()
    {
        Vector3 direction = (baseGravityCenter - transform.position).normalized;
        Vector3 gravityForce = direction * gravityStrength;
        rb.AddForce(gravityForce);
    }
}
