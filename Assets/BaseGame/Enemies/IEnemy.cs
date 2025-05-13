using LCPS.SlipForge.Enum.Enemy;
using System.Collections;
using UnityEngine;

namespace LCPS.SlipForge.Enemy
{
    public interface IEnemy
    {
        void AssignMovementSet(EnemyMovementSet set);
        EnemyType GetEnemyType();
    }
}