using LCPS.SlipForge.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDefinitionData", menuName = "BaseGame/Enemies/EnemyDefinitionData")]
public class EnemyDefinitionData : ScriptableObject
{
    public string Name;
    [TextArea(3, 10)]
    public string Description;
    public Enemy Prefab;
}
