using LCPS.SlipForge.Enemy.AI;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Enemy
{
    [ExecuteInEditMode]
    public class EnemyMovementDebugger : MonoBehaviour
    {
#if UNITY_EDITOR

        private EnemyMovementSet _movementSet;
        private Transform _target;

        [SerializeField]
        private int futureSteps = 10;
        [SerializeField]
        private float timeStep = 0.2f;
        [SerializeField]
        private Color futurePathColor = Color.blue;
        [SerializeField]
        private Color circleColor = Color.green; // Color for distance circles
        [SerializeField]
        private float circleInterval = 5f; // Distance interval for circles around the target
        [SerializeField]
        private bool DrawDistance;

        private int numberOfCircles = 5;
        private int circleResolution = 50;


        private Transform simulatedEnemy; // A simulated enemy for future calculations

        private void OnEnable()
        {
            var enemy = GetComponent<MovementSetSeekBehavior>();

            Assert.IsNotNull(enemy);

            _movementSet = enemy.MovementSet;

            Assert.IsNotNull(_movementSet);

            _target = enemy.Target;
        }

        private void OnDrawGizmos()
        {
            // Only draw when playing if the objct is selected
            //if (Application.isPlaying && !UnityEditor.Selection.Contains(gameObject))
            //    return;

            if (_movementSet == null || _target == null)
                return;

            // Draw rings around the target every 5 units to make it easier to visualize distance to target.
            if (DrawDistance)
            {
                DrawDistanceCircles();
            }


            // Initialize the simulated enemy's position to the current enemy's position
            if (simulatedEnemy == null)
            {
                var simulatedEnemyObject = new GameObject("SimulatedEnemy");
                simulatedEnemy = simulatedEnemyObject.transform;
            }
            simulatedEnemy.position = transform.position;

            Gizmos.color = futurePathColor;

            // Simulate future movement iterations
            for (int i = 0; i < futureSteps; i++)
            {
                // Calculate the movement based on the current simulated position
                var movement = _movementSet.CalculateMovement(simulatedEnemy, _target);

                // If there's any movement, calculate the next position
                if (movement != Vector3.zero)
                {
                    // Calculate the next position of the simulated enemy
                    var nextPosition = simulatedEnemy.position + movement * timeStep;

                    // Draw a line between the current simulated position and the next one
                    Gizmos.DrawLine(simulatedEnemy.position, nextPosition);

                    // Move the simulated enemy to the next position for the next iteration
                    if(!float.IsNaN(nextPosition.x))
                    {
                        simulatedEnemy.position = nextPosition;
                    }
                }
            }

            // Clean up the simulated enemy object after use in the editor
            if (simulatedEnemy != null)
            {
                DestroyImmediate(simulatedEnemy.gameObject);
            }
        }

        private void DrawDistanceCircles()
        {
            Gizmos.color = circleColor;

            // Draw circles at intervals around the target
            for (int i = 1; i <= numberOfCircles; i++)
            {
                var radius = i * circleInterval;
                DrawCircleOnGround(_target.position, radius);
            }
        }

        private void DrawCircleOnGround(Vector3 center, float radius)
        {
            float angleStep = 360f / circleResolution;

            var previousPoint = Vector3.zero;

            // Manually draw circles around the position using lines.
            for (int i = 0; i <= circleResolution; i++)
            {
                var angle = i * angleStep * Mathf.Deg2Rad;
                var point = new Vector3(
                    center.x + Mathf.Cos(angle) * radius,
                    center.y,
                    center.z + Mathf.Sin(angle) * radius
                );

                if (i > 0)
                {
                    Gizmos.DrawLine(previousPoint, point);
                }

                previousPoint = point;
            }
        }
#endif
    }
}

