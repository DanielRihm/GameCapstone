namespace LCPS.SlipForge.UI
{
    public class PauseMenu : Menu
    {
        protected ActionMap _inputScheme;

        protected override void Start()
        {
            base.Start();

            _inputScheme = new ActionMap();
            // this action map is slightly more complex than normal since MenuManager
            // also listens for the back button
            _inputScheme.UI.Pause.performed += ctx => ToggleMenu();
            _inputScheme.Enable();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _inputScheme.Disable();
        }

        protected override void ToggleMenu()
        {
            if (!(IsOpen() || DataTracker.Instance.IsMenuOpen()))
            {
                Open();
            }
        }
    }
}
