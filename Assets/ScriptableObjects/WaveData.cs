using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WaveData", order = 1)]
public class WaveData : ScriptableObject
{
    public int[] waveCost;

    public bool canSpawnBoss;
}
