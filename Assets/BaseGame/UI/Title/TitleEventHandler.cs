using UnityEngine;
using UnityEngine.UI;

namespace LCPS.SlipForge.UI
{
    public class TitleEventHandler : MonoBehaviour
    {
        [SerializeField] private Button StartButton;
        [SerializeField] private Button SettingsButton;
        [SerializeField] private Button ExitButton;

        // Start is called before the first frame update
        void Start()
        {
            StartButton.onClick.AddListener(SceneManager.StartGame);
            SettingsButton.onClick.AddListener(SettingsHandler.OpenSettings);
            ExitButton.onClick.AddListener(SceneManager.ExitGame);
        }
    }
}
