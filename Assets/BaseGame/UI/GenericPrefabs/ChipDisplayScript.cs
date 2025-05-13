using LCPS.SlipForge.Engine;
using TMPro;
using UnityEngine;

namespace LCPS.SlipForge.UI
{
    public class ChipDisplayScript : MonoBehaviour
    {
        private const string CHIP_TEXT = "x{0} Potato Chips";

        [SerializeField] private TextMeshProUGUI ChipsText;

        private Observable<int> _chipObserver;

        private void Awake()
        {
            _chipObserver = DataTracker.GetObservable(data => data.SaveData.Currency.PotatoChips);
        }

        private void OnEnable()
        {
            _chipObserver.OnValueChanged += UpdateChipDisplay;
        }

        private void OnDisable()
        {
            _chipObserver.OnValueChanged -= UpdateChipDisplay;
        }

        private void UpdateChipDisplay(int chips)
        {
            ChipsText.text = string.Format(CHIP_TEXT, chips);
        }
    }
}
