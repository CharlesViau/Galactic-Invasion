using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Planets;
using UnityEngine;

public class SpellUI : MonoBehaviour
{
    [SerializeField] private GameObject previewCircle;

    private bool canBePlaced;
    private Material _material;
    
    private void Awake()
    {
        canBePlaced = true;
        _material = previewCircle.GetComponent<Renderer>().material;
    }

    private void Start()
    {
        _material.SetColor("_Color", new Color(0, 204, 0,100));
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Shield") || collider.CompareTag("Core"))
        {
            _material.SetColor("_Color", new Color(204, 0, 0,100));
            canBePlaced = false;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Shield") || collider.CompareTag("Core"))
        {
            _material.SetColor("_Color", new Color(0, 204, 0,100));
            canBePlaced = true;
        }
    }

    public bool CanBePlaced()
    {
        return canBePlaced;
    }
}
