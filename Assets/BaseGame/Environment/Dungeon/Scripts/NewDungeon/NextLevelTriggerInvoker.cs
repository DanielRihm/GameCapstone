namespace LCPS.SlipForge
{
    public class NextLevelTriggerInvoker : TriggerActionInvoker
    {
        public override void Interact()
        {
            DataTracker.Instance.SaveTrackerData();
            SceneManager.NextLevel();
        }
    }
}
