namespace LCPS.SlipForge
{
    public class SandboxTriggerInvoker : TriggerActionInvoker
    {
        public override void Interact()
        {
            SoundManager.Instance.PlaySFX("forge_sound");
            DataTracker.Instance.SaveTrackerData();
            SceneManager.BeginSandbox();
        }
    }
}
