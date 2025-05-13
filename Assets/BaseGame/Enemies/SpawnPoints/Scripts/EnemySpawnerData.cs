using LCPS.SlipForge.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnerData", menuName = "BaseGame/Enemies/EnemySpawnData")]
public class EnemySpawnerData : ScriptableObject
{
    public List<EnemySpawnRule> Pool = new();
}
