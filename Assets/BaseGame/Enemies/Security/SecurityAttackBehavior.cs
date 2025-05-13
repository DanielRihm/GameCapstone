using LCPS.SlipForge.Enemy.AI;
using LCPS.SlipForge.Weapon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Enemy
{
    public class SecurityAttackBehavior : MonoBehaviour, IEnemyState
    {
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private List<AudioClip> _shootEffects;

        // Start is called before the first frame update
        private void Start()
        {
            Assert.IsNotNull(_projectilePrefab, $"{this.name} requires a projectile prefab.");
            Assert.IsNotNull(PlayerInstance.Instance, $"{this.name} requires a PlayerController instance.");

            foreach (var sound in _shootEffects)
            {
                Assert.IsNotNull(sound, "AudioClip is null.");
                SoundManager.Instance.RegisterSFX(sound.name, sound);
            }
        }

        private void OnEnable()
        {
            var direction = (PlayerInstance.Instance.transform.position - transform.position).normalized;
            // Speed is apparently from magnitude
            direction *= _projectilePrefab.Speed;
            var projectile = Instantiate(_projectilePrefab, transform.position + Vector3.up, Quaternion.identity, transform.parent);
            projectile.Direction = direction;

            SoundManager.Instance.PlaySFX(_shootEffects[Random.Range(0, _shootEffects.Count)].name);
        }

        public EnemyBrain.EnemyBehaviorState GetStateType()
        {
            return EnemyBrain.EnemyBehaviorState.Attack;
        }
    }
}
