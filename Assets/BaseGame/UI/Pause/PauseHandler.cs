using UnityEngine;
using UnityEngine.UI;

namespace LCPS.SlipForge.UI
{
    public class PauseHandler : MonoBehaviour
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private GameObject PauseMenuObject;
        private IMenu pauseMenu;

        private void Start()
        {
            pauseMenu = PauseMenuObject.GetComponent<IMenu>();
            _resumeButton.onClick.AddListener(pauseMenu.Close);
            _settingsButton.onClick.AddListener(SettingsHandler.OpenSettings);
            _quitButton.onClick.AddListener(ReturnToTitle);
        }

        private void ReturnToTitle()
        {
            DataTracker.Instance.LeaveDungeonEvent(false);
            DataTracker.Instance.CloseAllMenus();
            SceneManager.ReturnToTitle();
        }
    }
}
