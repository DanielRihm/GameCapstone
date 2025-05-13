using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Enemy.AI
{
    public class HitStop : MonoBehaviour
    {
        public float HitStopTime = 0.1f;
        public float HitStopScale = 0.1f;

        private Animator _animator;
        private Enemy _self;
        private NavMeshAgent _agent;
        private float _animatorSpeed;
        private float _agentSpeed;
        private Coroutine _hitStopCoroutine;
        private float _hp; 

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _self = GetComponent<Enemy>();
            _agent = GetComponent<NavMeshAgent>();
            
            Assert.IsNotNull(_self, $"{this.name} requires an Enemy component.");
            Assert.IsNotNull(_animator, $"{this.name} requires an Animator component.");
            Assert.IsNotNull(_animator, $"{this.name} requires a NavMeshAgent component.");

            _hp = _self.Health.Value; // Event will fire immediatly
            _self.Health.OnValueChanged += OnHit;
            
            _hitStopCoroutine = null;
        }

        private void OnDisable()
        {
            _animatorSpeed = _animator.speed;
            _self.Health.OnValueChanged -= OnHit;
            StopAllCoroutines();
        }

        private void OnHit(float hp)
        {
            if(hp < _hp && _hitStopCoroutine == null)
            {
                _hp = hp;
                _hitStopCoroutine = StartCoroutine(PreformHitStop());
            }
        }
        
        private IEnumerator PreformHitStop()
        {

            _animatorSpeed = _animator.speed;
            _agentSpeed = _agent.speed;
            
            _animator.speed = HitStopScale;
            _agent.speed = _agentSpeed * HitStopScale;

            var start = Time.time;
            while(Time.time - start < HitStopTime)
            {
                // Jiggle the position a little bit
                transform.position += Random.insideUnitSphere * 0.1f;
                
                yield return null;
            }
            
            _agent.speed = _agentSpeed;
            _animator.speed = _animatorSpeed;

            _hitStopCoroutine = null;
        }
    }
}
