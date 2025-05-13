using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Enemy
{
    public class EnemyHitSfx : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> HitSounds;
        [SerializeField] private float Cooldown = 4;
        private Enemy _self;
        private Coroutine _playRoutine;

        private float _lastHp = 0;

        // Start is called before the first frame update
        private void Awake()
        {
            _self = GetComponent<Enemy>();

            Assert.IsNotNull(_self, $"{this.name} requires an Enemy component.");
            Assert.IsNotNull(SoundManager.Instance, "SoundManager is not present in the scene.");

            foreach(var sound in HitSounds)
            {
                SoundManager.Instance.RegisterSFX(sound.name, sound);
            }
        }

        private void OnEnable()
        {
            _lastHp = _self.Health.Value;
            _self.Health.OnValueChanged += OnHit;
        }

        private void OnHit(float hp)
        {
            if (hp >= _lastHp)
            {
                return;
            }

            _lastHp = hp;

            _playRoutine ??= StartCoroutine(PlaySound());
        }

        private IEnumerator PlaySound()
        {
            if (HitSounds == null || HitSounds.Count == 0)
            {
                Debug.LogWarning($"No Hit Sounds assigned to {gameObject.name} {this.name}");
                yield break;
            }

            var sound = HitSounds[UnityEngine.Random.Range(0, HitSounds.Count)];
            SoundManager.Instance.PlaySFX(sound.name);

            yield return new WaitForSeconds(Cooldown);

            _playRoutine = null;
        }

        private void OnDisable()
        {
            _self.Health.OnValueChanged -= OnHit;
            StopAllCoroutines();
        }
    }
}
