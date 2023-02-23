using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Planets;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Enemy : MonoBehaviour
{
    public int damage;
    
    [SerializeField] private Vector3 rotationSpeed;
    [SerializeField] private float gravityStrength = 3f;
    [SerializeField] private int hp;
    
    private Vector3 baseGravityCenter;
    private Rigidbody rb;
    private bool isAffected;
    private Gravity gravity;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        baseGravityCenter = GameObject.Find("MotherBase").transform.position;
    }
    void Start()
    {
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

    private void InGravity()
    {
        Vector3 direction = (baseGravityCenter - transform.position).normalized;
        Vector3 gravityForce = direction * gravityStrength;
        rb.AddForce(gravityForce);
    }
    
    public void ChangeGravity(Gravity newGravity)
    {
        gravity = newGravity;
        rb.velocity = Vector3.zero;
        isAffected = true;
    }

    public void Hit(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            //animation?
            Destroy(gameObject);
        }
    }
}
