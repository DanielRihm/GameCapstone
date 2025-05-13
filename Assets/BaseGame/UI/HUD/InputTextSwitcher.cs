using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LCPS.SlipForge.UI
{
    public class InputTextSwitcher : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI TextObject;
        [SerializeField] private string Text = "Press {0} to ...";
        [SerializeField] private string KeyboardInput = "E";
        [SerializeField] private string ControllerInput = "<sprite name=\"controller_r_pad_down\">";

        private void Awake()
        {
            DataTracker.Instance.OnDeviceChange += UpdateText;
            UpdateText();
        }

        private void OnDestroy()
        {
            if (DataTracker.Instance != null)
                DataTracker.Instance.OnDeviceChange -= UpdateText;
        }

        public void UpdateText(InputDevice _ = null)
        {
            if (DataTracker.Instance.IsKeyboardAndMouse)
                TextObject.text = string.Format(Text, KeyboardInput);
            else
                TextObject.text = string.Format(Text, ControllerInput);
        }
    }
}
