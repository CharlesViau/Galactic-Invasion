using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject fracturedPlanet;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        FractureObject();
    }

    private void FractureObject()
    {
        Instantiate(fracturedPlanet, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
