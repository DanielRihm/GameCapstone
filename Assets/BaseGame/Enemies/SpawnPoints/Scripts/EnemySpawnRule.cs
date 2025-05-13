using LCPS.SlipForge.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnRule", menuName = "BaseGame/Enemies/SpawnRule")]
public class EnemySpawnRule : ScriptableObject
{
    public EnemyDefinitionData EnemyDefinition;
    public EnemyMovementSet Moves;
}
