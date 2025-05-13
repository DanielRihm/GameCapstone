using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LCPS.SlipForge.UI
{
    [RequireComponent(typeof(Slider))]
    public class SliderTextUpdater : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private string prefix = "";
        [SerializeField] private string suffix = "";
        [SerializeField] private bool percentage = true;

        public void UpdateText(float value)
        {
            text.text = prefix + (percentage ? Mathf.RoundToInt(value * 100) : value).ToString() + suffix;
        }

        private void Start()
        {
            GetComponent<Slider>().onValueChanged.AddListener(UpdateText);
        }
    }
}
