using UnityEngine;

namespace LCPS.SlipForge.Enemy
{
    public class ZigZagBehavior : PursuitBehavior
    {
        public float Frequency = 1f; // How fast the zig-zag pattern happens
        public float Amplitude = 1f; // How wide the zig-zag is

        private float timeOffset; // Used to offset the sine wave

        private void Start()
        {
            // Set a random time offset to avoid identical behavior for multiple enemies
            timeOffset = Random.Range(0f, 2f * Mathf.PI);
        }

        public override Vector3 CalculateMovement(Transform enemy, Transform target)
        {
            // Calculate the forward direction toward the target
            var directionToTarget = (target.position - enemy.position).normalized;

            // Calculate the side-to-side motion using a sine wave
            var distanceToTarget = Vector3.Distance(enemy.position, target.position);
            // TODO: Problem, distance to target causes the eneme's path to speed as as the player approaches. -js
            var zigZagOffset = Mathf.Sin((distanceToTarget + timeOffset) * Frequency) * Amplitude;

            // Use the cross product to get the right direction for side-to-side motion
            var right = Vector3.Cross(Vector3.up, directionToTarget).normalized;

            // Combine the forward movement and the zig-zag motion
            var zigZagMovement = right * zigZagOffset;
            var forwardMovement = directionToTarget * Time.deltaTime;

            // Final movement is the sum of forward and zig-zag movements
            return forwardMovement + zigZagMovement;
        }
    }

}