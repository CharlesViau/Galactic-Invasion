using System;
using System.Collections;
using System.Collections.Generic;
using Ability;
using Core;
using UnityEngine;

public class SpellUI : MonoBehaviour, IPoolable, ICreatable<SpellUI.Args>
{
    private float radius;
    private Transform _toFollow;
    
    private void Awake()
    {
        //maybe radius * scale?
        radius = GetComponent<SphereCollider>().radius;
    }

    public void Update()
    {
        transform.position = _toFollow.position;
    }

    public class Args : ConstructionArgs
    {
        public Transform ToFollow { get; private set; }
        public Args(Vector3 spawningPosition, Transform toFollow) : base(spawningPosition)
        {
            this.ToFollow = toFollow;
        }
    }

    public ValueType ValueType => type;

    [SerializeField] private SpellUIType type;
    public void Pool()
    {
        gameObject.SetActive(false);
    }

    public void Depool()
    {
        gameObject.SetActive(true);
    }

    public void Construct(Args constructionArgs)
    {
        _toFollow = constructionArgs.ToFollow;
    }
}
