using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Enemy.AI
{
    public class AnomalyAttackBehavior : MonoBehaviour, IEnemyState 
    {
        [Range(0.1f, 100f)]
        public float LungeSpeed = 1f;
        public float LungeDistance = 2f;

        private Enemy _self;
        private NavMeshAgent _agent;
        private Coroutine _lungeRoutine;

        private void Awake()
        {
            _self = GetComponent<Enemy>();
            _agent = GetComponent<NavMeshAgent>();
            Assert.IsNotNull(_self, $"{this.name} requires an Enemy component");
            Assert.IsNotNull(_agent, $"{this.name} requires a NavMeshAgent component");
        }

        void OnEnable()
        {
            _lungeRoutine = StartCoroutine(Lunge());
        }

        void OnDisable()
        {
            if (_lungeRoutine != null)
            {
                StopCoroutine(_lungeRoutine);
            }
        }

        public EnemyBrain.EnemyBehaviorState GetStateType()
        {
            return EnemyBrain.EnemyBehaviorState.Attack;
        }

        private IEnumerator Lunge()
        {
            var direction = PlayerInstance.Instance.transform.position - transform.position;
            var distance = LungeDistance;
            var destination = transform.position + direction.normalized * distance;

            var totalTime = distance / LungeSpeed;
            var elapsedTime = 0f;

            while (elapsedTime < totalTime)
            {
                var step = Vector3.MoveTowards(transform.position, destination, LungeSpeed * Time.deltaTime);
                _agent.Warp(step);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _self.State.Value = EnemyBrain.EnemyBehaviorState.Seek;
        }
    }
}
