using System.Collections.Generic;
using System.Linq.Expressions;
using LCPS.SlipForge.Enemy;
using LCPS.SlipForge.Enemy.AI;
using UnityEngine;
using UnityEngine.AI;

public class PingPong : PursuitBehavior
{
    private Dictionary<Transform, MovementSetSeekBehavior> directionMap = new();

    public override Vector3 CalculateMovement(Transform enemy, Transform player)
    {
        if (!directionMap.ContainsKey(enemy) && enemy.GetComponent<MovementSetSeekBehavior>() is MovementSetSeekBehavior newSeek)
        {
            directionMap.Add(enemy, newSeek);
        }

        if (!directionMap.TryGetValue(enemy, out MovementSetSeekBehavior seek))
        {
            return Vector3.zero;
        }

        if (seek.Direction == Vector3.zero)
        {
            // Randomize direction biased to 45 degree angles
            var direction = new Vector3(Random.Range(0.25f, 0.75f), 0, Random.Range(0.25f, 0.75f));
            seek.Direction = direction.normalized * (Random.Range(0, 2) == 0 ? -1 : 1);
        }

        // Refelect movement off the edge of the nav mesh
        if (NavMesh.Raycast(enemy.position, enemy.position + seek.Direction, out var hit, NavMesh.AllAreas))
        {
            var reflect = Vector3.Reflect(seek.Direction, hit.normal);
            seek.Direction = reflect.normalized;
        }

        return seek.Direction;
    }
}
