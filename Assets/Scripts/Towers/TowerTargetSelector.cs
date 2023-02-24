using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTargetSelector : MonoBehaviour
{
    [SerializeField]
    private float detectionRange;

    public float DetectionRange => detectionRange;
    
    public void GetClosestEnemy()
    {
        Helper.GetClosetInRange(typeof(Enemy), transform, detectionRange);
    }
}
