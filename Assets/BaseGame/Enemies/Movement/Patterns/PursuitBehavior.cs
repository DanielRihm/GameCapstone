using UnityEngine;

namespace LCPS.SlipForge.Enemy
{
    public abstract class PursuitBehavior : ScriptableObject
    {
        public AnimationCurve Curve = AnimationCurve.Linear(0, 0, 1, 1);  // Default curve
        public float MaxRange = 1f;
        public float MinRange = 0f;

        public abstract Vector3 CalculateMovement(Transform enemy, Transform player);

        // Check if the player is within range
        public bool IsInRange(Transform enemy, Transform player)
        {
            var distance = Vector3.Distance(enemy.position, player.position);
            return distance >= MinRange && distance <= MaxRange;
        }
    }

}

