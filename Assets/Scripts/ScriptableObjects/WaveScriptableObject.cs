using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class WaveScriptableObject : ScriptableObject
{
    public EnemyData normal;
    public EnemyData fast;
    public EnemyData slow;
}


[Serializable]
public class EnemyData
{
    public int numberOfEnemies;

    public bool newSpawner;
}
