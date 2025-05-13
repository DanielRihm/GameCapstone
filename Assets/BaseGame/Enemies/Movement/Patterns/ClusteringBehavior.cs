using LCPS.SlipForge.Enum.Enemy;
using UnityEngine;

namespace LCPS.SlipForge.Enemy
{
    public class ClusteringBehavior : PursuitBehavior
    {
        [SerializeField]
        private EnemyType Group; // Defines the enemy type this behavior applies to

        [SerializeField]
        private float ClusterDistance = 3f; // The ideal distance between enemies in the group

        [SerializeField]
        private float ClusterTollerance = 1f; // The tolerance for the distance variation

        // Layer mask for detecting enemies
        [SerializeField]
        private LayerMask LayerMask;

        // Buffer for non-allocating OverlapSphere
        private Collider[] collidersBuffer = new Collider[10];

        public override Vector3 CalculateMovement(Transform enemy, Transform player)
        {
            var separationForce = Vector3.zero;
            int nearbyCount = 0;

            // Perform an OverlapSphereNonAlloc around the enemy to detect nearby enemies within ClusterDistance
            int numColliders = Physics.OverlapSphereNonAlloc(enemy.position, ClusterDistance, collidersBuffer, LayerMask);

            for (int i = 0; i < numColliders; i++)
            {
                var hitCollider = collidersBuffer[i];
                var thisCollider = enemy.GetComponent<Collider>();

                if (hitCollider != null && hitCollider.transform != enemy && hitCollider != thisCollider)
                {
                    // Get the other enemy's transform
                    var otherTransform = hitCollider.transform;

                    // Check if the detected enemy belongs to the same group (using EnemyType Group)
                    var otherEnemy = otherTransform.GetComponent<IEnemy>();
                    if (otherEnemy != null && otherEnemy.GetEnemyType() == Group)
                    {
                        // Calculate the distance between this enemy and the other enemy
                        var distance = Vector3.Distance(enemy.position, otherTransform.position);

                        // Are we too close?
                        if (distance < ClusterDistance - ClusterTollerance)
                        {
                            // Push the enemy away from the other enemy
                            var directionAway = (enemy.position - otherTransform.position).normalized;
                            separationForce += directionAway * (ClusterDistance - distance);
                        }
                        // Are we too far?
                        else if (distance > ClusterDistance + ClusterTollerance)
                        {
                            // Pull the enemy closer to the other enemy
                            var directionTowards = (otherTransform.position - enemy.position).normalized;
                            separationForce -= directionTowards * (distance - ClusterDistance);
                        }

                        nearbyCount++;
                    }
                }
            }

            // Average the change for multiple enemies
            if (nearbyCount > 0)
            {
                separationForce /= nearbyCount;
            }

            // Return the calculated separation movement to adjust the enemy's position
            return separationForce;
        }
    }
}
