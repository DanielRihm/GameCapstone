namespace LCPS.SlipForge.Enemy
{
    using UnityEngine;
    using System.Collections.Generic;

    [CreateAssetMenu(menuName = "PursuitBehaviors/EnemyMovementSet")]
    public class EnemyMovementSet : ScriptableObject
    {
        public List<PursuitBehavior> Behaviors;

        private void OnEnable()
        {
            // Ensure the list is initialized
            if (Behaviors == null)
            {
                Behaviors = new List<PursuitBehavior>();
            }
        }

        public Vector3 CalculateMovement(Transform enemy, Transform target)
        {
            var validBehaviors = new List<PursuitBehavior>();
            var targetDistance = Vector3.Distance(enemy.position, target.position);

            // Only consider behaviors that are within their actionable range
            foreach (var behavior in Behaviors)
            {
                if (targetDistance >= behavior.MinRange && targetDistance <= behavior.MaxRange)
                {
                    validBehaviors.Add(behavior);
                }
            }

            // If no behaviors are valid, return no movement
            if (validBehaviors.Count == 0)
            {
                return Vector3.zero;
            }

            var weights = new List<float>(new float[validBehaviors.Count]);
            float totalWeight = 0f;

            // Compute a weight for each behavior using its weightCurve
            for (int i = 0; i < validBehaviors.Count; i++)
            {
                var behavior = validBehaviors[i];

                // Normalize the distance for the current behavior's range
                var normalizedDistance = Mathf.InverseLerp(behavior.MinRange, behavior.MaxRange, targetDistance);

                // Use the behavior's weight curve to get the weight for the current distance
                var weight = behavior.Curve.Evaluate(normalizedDistance);

                // Store the weight and accumulate totalWeight
                weights[i] = weight;
                totalWeight += weight;
            }

            // Step 2: Normalize the weights so they sum to 1
            for (int i = 0; i < weights.Count; i++)
            {
                weights[i] /= totalWeight; // Normalize each weight by the total weight
            }

            var blendedMovement = Vector3.zero;

            // Basically boids now
            for (int i = 0; i < validBehaviors.Count; i++)
            {
                var movement = validBehaviors[i].CalculateMovement(enemy, target);
                blendedMovement += movement * weights[i]; // Apply the weighted movement
            }

            return blendedMovement; // Return the final blended movement vector
        }
    }

}
