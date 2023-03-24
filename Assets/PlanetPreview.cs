using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetPreview : MonoBehaviour
{
    private float radius;
    
    private void Awake()
    {
        //maybe radius * scale?
        radius = GetComponent<SphereCollider>().radius;
    }

}
