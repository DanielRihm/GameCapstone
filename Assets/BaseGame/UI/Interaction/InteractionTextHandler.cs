using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LCPS.SlipForge.UI
{
    public class InteractionTextHandler : MonoBehaviour
    {
        private const string KEYBOARD_TEXT = "Press E to ";
        private const string CONTROLLER_TEXT = "Press <sprite name=\"r_pad_down\"> to ";

        [SerializeField] private GameObject keyboardInteraction;
        [SerializeField] private GameObject controllerInteraction;
        private TextMeshProUGUI keyboardText;
        private TextMeshProUGUI controllerText;
        private string _interactionTag;
        public string InteractionTag
        {
            get => _interactionTag;
            set => SetText(value);
        }

        private void Awake()
        {
            keyboardText = keyboardInteraction.GetComponentInChildren<TextMeshProUGUI>();
            controllerText = controllerInteraction.GetComponentInChildren<TextMeshProUGUI>();
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
            keyboardInteraction.SetActive(DataTracker.Instance.IsKeyboardAndMouse);
            controllerInteraction.SetActive(!DataTracker.Instance.IsKeyboardAndMouse);
        }

        private void SetText(string text)
        {
            Debug.Log("Setting interaction text to: " + text);
            _interactionTag = text;
            keyboardText.text = KEYBOARD_TEXT + text;
            controllerText.text = CONTROLLER_TEXT + text;
        }
    }
}
