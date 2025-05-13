using LCPS.SlipForge.Engine;
using LCPS.SlipForge.Weapon;
using UnityEngine;
using UnityEngine.UI;

namespace LCPS.SlipForge.UI
{
    [RequireComponent(typeof(Button))]
    public class SwapButton : MonoBehaviour
    {
        private Observable<WeaponData> _leftWeaponObserver;
        private Observable<WeaponData> _rightWeaponObserver;

        void Start()
        {
            GetComponent<Button>().onClick.AddListener(Swap);

            _leftWeaponObserver = DataTracker.GetObservable(data => data.LeftWeapon);
            _rightWeaponObserver = DataTracker.GetObservable(data => data.RightWeapon);
        }

        public void Swap()
        {
            (_rightWeaponObserver.Value, _leftWeaponObserver.Value) = (_leftWeaponObserver.Value, _rightWeaponObserver.Value);
        }
    }
}
