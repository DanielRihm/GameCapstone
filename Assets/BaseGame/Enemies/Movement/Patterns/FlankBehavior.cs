using LCPS.SlipForge.Enum.Enemy;
using UnityEngine;

namespace LCPS.SlipForge.Enemy
{
    public class FlankBehavior : PursuitBehavior
    {
        [SerializeField]
        private FlankDirection Direction;

        public override Vector3 CalculateMovement(Transform enemy, Transform player)
        {
            // Calculate the direction from the enemy to the player
            var directionToPlayer = (player.position - enemy.position).normalized;

            // Calculate the vector perpindicular to the direction to the player
            var flank = Vector3.Cross(Vector3.up, directionToPlayer).normalized;

            // Which way to flank?
            if (Direction == FlankDirection.Left)
            {
                flank *= -1;
            }

            // Return the calculated flank movement to the right
            return flank;
        }
    }
}
