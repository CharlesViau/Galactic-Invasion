using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrail : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float progress;

    [SerializeField] private float speed = 200f;

    private void FixedUpdate()
    {
        progress += Time.deltaTime * speed;
        transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
    }

    public void SetTargetPosition(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
    }
}
