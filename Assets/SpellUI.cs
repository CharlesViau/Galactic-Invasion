using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Planets;
using UnityEngine;

public class SpellUI : MonoBehaviour
{
    private float radius;
    private Transform _toFollow;
    [SerializeField] private BlackHole blackHole;
    
    private void Awake()
    {
        //maybe radius * scale?
        radius = GetComponent<SphereCollider>().radius;
    }

}
