using LCPS.SlipForge.Engine;
using LCPS.SlipForge.Weapon;
using System.Collections;
using UnityEngine;

namespace LCPS.SlipForge.UI
{
    public class WeaponHUD : ToolHUD
    {
        [SerializeField] private bool _isLeftWeapon = false;
        private Observable<WeaponData> _weaponObserver;
        private Observable<int> _ammoObserver;
        private Observable<bool> _reloadingObserver;

        private Coroutine _reloadCoroutine;

        protected override void Start()
        {
            base.Start();
            _weaponObserver = DataTracker.GetObservable(data => _isLeftWeapon ? data.LeftWeapon : data.RightWeapon);
            _weaponObserver.Subscribe(UpdateWeaponRef);

            _ammoObserver = DataTracker.GetObservable(data => _isLeftWeapon ? data.LeftWeaponAmmoCount : data.RightWeaponAmmoCount);
            _ammoObserver.Subscribe(UpdateAmmoCount);

            _reloadingObserver = DataTracker.GetObservable(data => _isLeftWeapon ? data.LeftWeaponReloading : data.RightWeaponReloading);
            _reloadingObserver.Subscribe(UpdateReloading);
        }

        private void OnDestroy()
        {
            _weaponObserver.UnSubscribe(UpdateWeaponRef);
            _ammoObserver.UnSubscribe(UpdateAmmoCount);
            _reloadingObserver.UnSubscribe(UpdateReloading);
        }

        private void UpdateReloading(bool reloading)
        {
            if (!reloading)
            {
                if (_reloadCoroutine != null)
                {
                    StopCoroutine(_reloadCoroutine);
                    _reloadCoroutine = null;
                }
                UpdateWeaponRef(_weaponObserver.Value);
                return;
            }
            _reloadCoroutine = StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            _maxAmmo = _weaponObserver.Value.ReloadTime;
            float time = 0;
            while (time < _maxAmmo)
            {
                time += Time.deltaTime;
                UpdateAmmoCount(_maxAmmo - time);
                yield return null;
            }
        }

        private void UpdateWeaponRef(WeaponData data)
        {
            Icon.sprite = data != null ? data.Sprite : _transparentSprite;
            _maxAmmo = data != null ? data.AmmoCount : 0;
            UpdateAmmoCount(data != null ? _ammoObserver.Value : 0);
        }
    }
}
