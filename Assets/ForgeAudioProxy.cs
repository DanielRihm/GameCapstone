using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCPS.SlipForge
{
    public class ForgeAudioProxy : MonoBehaviour
    {
        [SerializeField] private AudioClip FizzleSound;
        [SerializeField] private AudioClip SpinSound;
        [SerializeField] private AudioClip StartSound;

        // Start is called before the first frame update
        void Start()
        {
            SoundManager.Instance.RegisterSFX(ForgeSounds.ForgeFizzle.ToString(), FizzleSound);
            SoundManager.Instance.RegisterSFX(ForgeSounds.ForgeSpin.ToString(), SpinSound);
            SoundManager.Instance.RegisterSFX(ForgeSounds.ForgeStart.ToString(), StartSound);
        }

        public void PlaySound(ForgeSounds sound)
        {
            SoundManager.Instance.PlaySFX(sound.ToString());
        }

        public void OnDisable()
        {
            SoundManager.Instance.StopSfx();
        }
    }
    public enum ForgeSounds
    {
        ForgeFizzle,
        ForgeSpin,
        ForgeStart
    }
}
