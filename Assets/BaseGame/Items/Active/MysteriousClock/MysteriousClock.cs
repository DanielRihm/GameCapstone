using UnityEngine;

namespace Assets.BaseGame.Guns.Scripts
{
    public class MysteriousClock : AbstractActiveItem
    {
        public float SlowMultiplier = 2.0f;

		protected override void Start()
        {
            AbstractCooldownSeconds = Data.CooldownSeconds;
            AbstractDurationSeconds = Data.DurationSeconds / SlowMultiplier;
            base.Start();
        }

        protected override void DoEffects()
        {
            Time.timeScale /= SlowMultiplier;
            SoundManager.Instance.AudioMixer.SetFloat("MasterPitch",0.5f);
        }

        protected override void StopEffects()
        {
			SoundManager.Instance.AudioMixer.SetFloat("MasterPitch", 1);
            Time.timeScale *= SlowMultiplier;
        }
    }
}