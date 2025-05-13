using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Enemy.AI
{
    public class MovementSetSeekBehavior : MonoBehaviour, IEnemyState
    {
        public EnemyMovementSet MovementSet;

        private NavMeshAgent _navAgent;

        public Transform Target => PlayerInstance.Instance.transform;

        public float MinMovement;

        public Vector3 Direction { get; set; }

        private void Awake()
        {
            _navAgent = GetComponent<NavMeshAgent>();

            Assert.IsNotNull(_navAgent);
        }

        private void OnEnable()
        {
            _navAgent.isStopped = false;
        }

        private void OnDisable()
        {
            if(_navAgent == null)
            {
                return;
            }

            if(_navAgent != null && _navAgent.isOnNavMesh)
            {
                _navAgent.isStopped = true;
            }
        }

        private void Update()
        {
            if (PlayerInstance.Instance == null || Target == null || MovementSet == null)
            {
                _navAgent.ResetPath(); // Stop the agent if no target
                return;
            }

            // Use the EnemyMovementSet to calculate the next movement direction
            var movementDirection = MovementSet.CalculateMovement(transform, Target);

            // Set the NavMeshAgent destination based on the calculated direction
            if (movementDirection.magnitude != MinMovement)
            {
                _navAgent.SetDestination(transform.position + movementDirection.normalized);
            }
        }

        public EnemyBrain.EnemyBehaviorState GetStateType()
        {
            return EnemyBrain.EnemyBehaviorState.Seek;
        }
    }
}
