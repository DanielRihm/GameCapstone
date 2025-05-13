using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Enemy.AI
{
    public class AnomalyTelegraphBehavior : MonoBehaviour, IEnemyState
    {
        private Enemy _self;
        private Coroutine _telegraphRoutine;

        private void Start()
        {
            _self = GetComponent<Enemy>();
            Assert.IsNotNull(_self, $"{this.name} requires an Enemy component");
        }

        private void Awake()
        {
            _self = GetComponent<Enemy>();
            Assert.IsNotNull(_self, $"{this.name} requires an Enemy component");
        }

        void OnEnable()
        {
            // Start the telegraph animation
            _telegraphRoutine = StartCoroutine(RotateTowardsPlayer());
        }

        private IEnumerator RotateTowardsPlayer()
        {
            var direction = PlayerInstance.Instance.transform.position - transform.position;
            var rotation = Quaternion.LookRotation(direction);
            while (Quaternion.Angle(transform.rotation, rotation) > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                yield return null;
            }
        }

        void OnDisable()
        {
            // Stop the telegraph animation
            if (_telegraphRoutine != null)
            {
                StopCoroutine(_telegraphRoutine);
            }
        }

        public EnemyBrain.EnemyBehaviorState GetStateType()
        {
            return EnemyBrain.EnemyBehaviorState.Telegraph;
        }
    }
}
