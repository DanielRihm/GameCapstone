using UnityEngine;

namespace LCPS.SlipForge.Enemy
{
    public class RushBehavior : PursuitBehavior
    {
        [SerializeField]
        private bool Flee;

        public override Vector3 CalculateMovement(Transform enemy, Transform player)
        {
            return (Flee ? -1 : 1) * (player.position - enemy.position).normalized;
        }
    }

}
