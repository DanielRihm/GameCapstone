using LCPS.SlipForge.Enemy;
using LCPS.SlipForge.Enemy.AI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge
{
    public class ScavengerAttackBehavior : BehaviorBase
    {
        [SerializeField] private Transform _attackObject;
        [SerializeField] private List<AudioClip> _audioSources;

        void Start()
        {
            Assert.IsNotNull(PlayerInstance.Instance, "PlayerInstance is null.");
            Assert.IsNotNull(_attackObject, "AttackObject is null.");
            Assert.IsNotNull(_audioSources, "AudioClip is null.");

            foreach(var sound in _audioSources)
            {
                SoundManager.Instance.RegisterSFX(sound.name, sound);
            }
        }

        void OnEnable()
        {
            // Attack should be in the direction of the player
            var playerDirection = (PlayerInstance.Instance.transform.position - transform.position).normalized;
            _attackObject.forward = playerDirection;

            // Randomly play a sound
            var sound = _audioSources[Random.Range(0, _audioSources.Count)];
            SoundManager.Instance.PlaySFX(sound.name);
        }

        // Update is called once per frame
        void OnDisable()
        {
        }
    }
}
