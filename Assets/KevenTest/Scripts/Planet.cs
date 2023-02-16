using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private float timeToLive;

    private void FixedUpdate()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0)
        {
            //Play destruction animation
            //Spawn 10 enemies
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
