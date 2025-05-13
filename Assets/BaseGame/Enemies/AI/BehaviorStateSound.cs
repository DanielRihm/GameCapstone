using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Enemy.AI
{
    public class BehaviorStateSound : MonoBehaviour
    {
        public Enemy Self;

        [SerializeField] private EnemyBrain.EnemyBehaviorState ActivateState;
        public List<AudioClip> Sounds;
        public bool Loop = false;

        private bool _playing;

        // Start is called before the first frame update
        void Start()
        {
            Assert.IsNotNull(Self, $"{this.name} requires an Enemy component.");
            Assert.IsNotNull(Sounds, $"{this.name} requires a Sound clip.");
            Assert.IsNotNull(SoundManager.Instance, $"{this.name} requires a SoundManager.");

            foreach(var sound in Sounds)
            {
                if(sound != null)
                {
                    SoundManager.Instance.RegisterSFX(sound.name, sound);
                }
            }


            Self.State.OnValueChanged += OnStateChange;

        }

        private void OnStateChange(EnemyBrain.EnemyBehaviorState state)
        {
            if (state == ActivateState)
            {
                _playing = true;
                var sound = Sounds[Random.Range(0, Sounds.Count)];

                // We allow blanks
                if (sound == null)
                    return;

                // Special handeling for death sounds
                // Fights with all other things makign sounds but it's better than sounds
                // living for too long.
                if (state == EnemyBrain.EnemyBehaviorState.Die)
                {
                    SoundManager.Instance.StopSfx();
                }

                if (Loop)
                {
                    StopAllCoroutines();
                    StartCoroutine(PlayRepeat(sound));
                }
                else
                {
                    Debug.Log($"Playing {state} sound {sound.name}");
                    SoundManager.Instance.PlaySFX(sound.name);
                }
            }
            else
            {
                _playing = false;
            }
        }

        private IEnumerator PlayRepeat(AudioClip clip)
        {
            while(_playing)
            {
                SoundManager.Instance.PlaySFX(clip.name);
                yield return new WaitForSeconds(clip.length);
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();

            _playing = false;
        }
    }
}
