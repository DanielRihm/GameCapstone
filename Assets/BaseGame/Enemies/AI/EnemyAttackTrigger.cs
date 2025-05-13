using UnityEngine;
using UnityEngine.Assertions;
using static EnemyBrain;

namespace LCPS.SlipForge.Enemy.AI
{
    [RequireComponent(typeof(EnemyBrain))]
    public class EnemyAttackTrigger : MonoBehaviour
    {
        public float Cooldown = 7f;
        [SerializeField] private Collider TriggerCollider;

        private Transform Target => PlayerInstance.Instance.transform;
        private Enemy _self;
        private float _cooldown;

        void Start()
        {
            _self = GetComponent<Enemy>();
            Assert.IsNotNull(TriggerCollider, $"{this.name} requires a collider to trigger attacks");
            Assert.IsNotNull(_self, $"{this.name} requires an Enemy component");

            // This component is only active when the enemy is in the seek state
            enabled = false;
            _self.State.OnValueChanged += OnStateChanged;
        }

        private void OnEnable()
        {
            _cooldown = Random.Range(0, Cooldown);
        }

        private void OnStateChanged(EnemyBehaviorState state)
        {
            enabled = state == EnemyBehaviorState.Seek;
        }

        void Update()
        {
            if (_cooldown >= 0)
            {
                _cooldown -= Time.deltaTime;
                return;
            }

            // Test for player in collider
            if (TriggerCollider.bounds.Contains(Target.position))
            {
                _self.State.Value = EnemyBrain.EnemyBehaviorState.Telegraph;
                _cooldown = Cooldown;
            }
        }
    }
}
