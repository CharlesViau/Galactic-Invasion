using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float cooldown;
    
    [SerializeField] private float range;

    [SerializeField] private float projectileSpeed;

    [SerializeField] private int damage;

    [SerializeField] private Projectile projectilePrefab;

    [SerializeField] private Transform gunPoint;

    [SerializeField] private GameObject bulletTrail;

    private Plane plane;

    private Vector3 target;

    private LayerMask motherBaseLayerMask;

    private LayerMask enemyLayerMask;

    private bool isReadyToFire;

    private void Start()
    {
        isReadyToFire = true;
        plane = new Plane(Vector3.back, 0);
        motherBaseLayerMask = LayerMask.GetMask("Motherbase");
        enemyLayerMask = LayerMask.GetMask("Enemy");
    }

    private void FixedUpdate()
    {
        
        if (Input.GetMouseButton(0) && isReadyToFire && HasLineOfSight())
        {
            Fire();
            StartCoroutine(Cooldown());
        }
    }

    private void Fire()
    {
        //Projectile
        Projectile projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.OnInstantiate(projectileSpeed, damage, target);
        
        //Raycast
        /*RaycastHit hit;
        var trail = Instantiate(bulletTrail, gunPoint.position, transform.rotation);
        var trailScript = trail.GetComponent<LaserTrail>();
        if (Physics.Raycast(gunPoint.position, (target - gunPoint.position).normalized, out hit, range,
                enemyLayerMask))
        {
            //Debug.DrawRay(transform.position, (target - gunPoint.position).normalized * hit.distance, Color.yellow);
            //hit.rigidbody.gameObject.GetComponent<Enemy>().Hit(damage);
            trailScript.SetTargetPosition(hit.point);
            Debug.DrawRay(transform.position, (target - gunPoint.position).normalized * hit.distance, Color.yellow);
            hit.rigidbody.gameObject.GetComponent<Enemy>().Hit(damage);
        }
        else
        {
            trailScript.SetTargetPosition(transform.position + (target - gunPoint.position).normalized * range);
            Debug.DrawRay(transform.position, (target - gunPoint.position).normalized * range, Color.red);
        }*/
    }

    private bool HasLineOfSight()
    {
        float distance;
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(mouseRay, out distance))
        {
            target = mouseRay.GetPoint(distance);
        }

        RaycastHit hit;
        if (Physics.Raycast(gunPoint.position, (target - gunPoint.position).normalized, out hit, range,
                motherBaseLayerMask))
        {
            //Debug.DrawRay(transform.position, (target - gunPoint.position).normalized * range, Color.red);
            return false;
        }
        
        return true;
    }

    private IEnumerator Cooldown()
    {
        isReadyToFire = false;
        yield return new WaitForSeconds(cooldown);
        isReadyToFire = true;
    }
}
