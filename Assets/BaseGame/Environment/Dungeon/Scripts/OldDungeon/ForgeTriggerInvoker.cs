using Unity;
using UnityEngine;

namespace LCPS.SlipForge
{
    [RequireComponent(typeof(Animator))]
    public class ForgeTriggerInvoker : TriggerActionInvoker
    {
        private bool _started;
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public override void Interact()
        {
            if (Tag == "Skip")
            {
                LoadDungeon();
                return;
            }

            if (_started) return;

            _animator.SetTrigger("Activate");
            _started = true;
            Tag = "Skip";
        }

        public void LoadDungeon()
        {
            SoundManager.Instance.PlaySFX("forge_sound");
            DataTracker.Instance.SaveTrackerData();
            SceneManager.BeginDungeon();
        }
    }
}
