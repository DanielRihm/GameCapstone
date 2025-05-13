using System.Collections;
using LCPS.SlipForge.Weapon;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Enemy.AI
{
    public class BossAttackBehavior : MonoBehaviour, IEnemyState
    {
        private Enemy _self;
        private MovementSetSeekBehavior _seekBehavior;
        private Vector3 _projectileDirection;
        [SerializeField] private BossProjectile _projectilePrefab;
        [SerializeField] private float AttackInterval = 1.0f;
        [SerializeField] private float AttackLength = 1.0f;
        [SerializeField] private AudioClip SpinDownClip;
        [SerializeField] private AudioClip ShootClip;

        public void Start()
        {
        }
        
        public void OnEnable()
        {
            if(_self == null)
                _self = GetComponent<Enemy>();
            if(_seekBehavior == null)
                _seekBehavior = GetComponent<MovementSetSeekBehavior>();

            Assert.IsNotNull(_self, $"{this.name} requires an Enemy component.");
            Assert.IsNotNull(_seekBehavior, $"{this.name} requires a MovementSetSeekBehavior component.");
            Assert.IsNotNull(_projectilePrefab, $"{this.name} requires a Projectile prefab.");
            Assert.IsNotNull(SpinDownClip, $"{this.name} requires a SpinDownClip.");
            Assert.IsNotNull(ShootClip, $"{this.name} requires a ShootClip.");
            Assert.IsNotNull(SoundManager.Instance, $"{this.name} requires a SoundManager.");

            SoundManager.Instance.RegisterSFX(SpinDownClip.name, SpinDownClip);
            SoundManager.Instance.RegisterSFX(ShootClip.name, ShootClip);

            // The boss will always move
            _seekBehavior.enabled = true;
            var startDirection = Random.insideUnitCircle;
            _projectileDirection = new Vector3(startDirection.x, 0, startDirection.y);
            StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            StartCoroutine(PlaySpinDown());
            var start = Time.time;
            while (Time.time - start < AttackLength)
            {
                // Rotate the attack direction.
                _projectileDirection = Quaternion.Euler(0, 15, 0) * _projectileDirection;

                // Projectiels shoud inherit the velocity of the boss
                var projectile = Instantiate(_projectilePrefab, transform.position + Vector3.up, Quaternion.identity, transform.parent);
                projectile.Forward = _projectileDirection;
                SoundManager.Instance.PlaySFX(ShootClip.name);

                // Fires in two directions at half health
                if (_self.Health.Value <= _self.MaxHP / 2)
                {
                    projectile = Instantiate(_projectilePrefab, transform.position + Vector3.up, Quaternion.identity, transform.parent);
                    projectile.Forward = -_projectileDirection;
                }

                yield return new WaitForSeconds(AttackInterval);
            }

            _self.State.Value = EnemyBrain.EnemyBehaviorState.Seek;
        }

        private IEnumerator PlaySpinDown()
        {
            var spindownTime = AttackLength - SpinDownClip.length;
            yield return new WaitForSeconds(spindownTime);

            SoundManager.Instance.PlaySFX(SpinDownClip.name);
        }

        public void OnDisable()
        {
            StopAllCoroutines();
            if(_self.State.Value != EnemyBrain.EnemyBehaviorState.Seek)
            {
                _seekBehavior.enabled = false;
            }
        }

        public EnemyBrain.EnemyBehaviorState GetStateType()
        {
            return EnemyBrain.EnemyBehaviorState.Attack;
        }
    }
}
