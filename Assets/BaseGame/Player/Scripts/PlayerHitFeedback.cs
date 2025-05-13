using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Player
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    [RequireComponent(typeof(PlayerInstance))]
    public class PlayerHitFeedback : MonoBehaviour
    {
        private CinemachineImpulseSource _impulseSource;
        private PlayerInstance _playerInstance;
        private int _lastHealth;

        [SerializeField] private AudioClip _hitSound;

        private void Start()
        {
            _impulseSource = GetComponent<CinemachineImpulseSource>();
            _playerInstance = GetComponent<PlayerInstance>();

            Assert.IsNotNull(_impulseSource, $"{this.name} requires a CinemachineImpulseSource component.");
            Assert.IsNotNull(_playerInstance, $"{this.name} requires a PlayerInstance component.");
            Assert.IsNotNull(_hitSound, $"{this.name} requires a hit sound.");
            Assert.IsNotNull(SoundManager.Instance, "SoundManager is null.");

            DataTracker.Subscribe(data => data.SaveData.HP, OnHealthChanged);
            SoundManager.Instance.RegisterSFX(_hitSound.name, _hitSound);
        }

        private void OnHealthChanged(int newHealth)
        {
            if (newHealth < _lastHealth)
            {
                GotHit();
            }

            _lastHealth = newHealth;
        }

        private void OnDisable()
        {
            DataTracker.UnSubscribe(data => data.SaveData.HP, OnHealthChanged);
        }

        public void GotHit()
        {
            SoundManager.Instance.PlaySFX(_hitSound.name);

            // The sound is slow for some reason -js
            StartCoroutine(DelayShake());
        }

        private IEnumerator DelayShake()
        {
            yield return new WaitForSeconds(0.2f);
            _impulseSource.GenerateImpulse();
        }
    }
}
