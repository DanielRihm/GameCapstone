using LCPS.SlipForge.Engine;
using UnityEngine;
using UnityEngine.UI;

namespace LCPS.SlipForge.UI
{
    [RequireComponent(typeof(Button))]
    public class MaxHealthBuyButton : MonoBehaviour
    {
        private Button _button;
        private Observable<int> _maxHealthObserver;
        private Observable<int> _chipObserver;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(BuyMaxHealth);

            _maxHealthObserver = DataTracker.GetObservable(data => data.MaxHealth);
            _chipObserver = DataTracker.GetObservable(data => data.SaveData.Currency.PotatoChips);
            _chipObserver.OnValueChanged += CheckChipCount;
        }

        private void OnDestroy()
        {
            _chipObserver.OnValueChanged -= CheckChipCount;
        }

        private void CheckChipCount(int chips)
        {
            _button.interactable = chips >= 100;
        }

        private void BuyMaxHealth()
        {
            _maxHealthObserver.Value += 1;
            _chipObserver.Value -= 100;
        }
    }
}
