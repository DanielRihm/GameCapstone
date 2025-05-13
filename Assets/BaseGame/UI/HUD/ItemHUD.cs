using LCPS.SlipForge.Engine;
using LCPS.SlipForge.Weapon;

namespace LCPS.SlipForge.UI
{
    public class ItemHUD : ToolHUD
    {
        private Observable<ActiveData> _itemObserver;
        private Observable<float> _cooldownObserver;

        protected override void Start()
        {
            base.Start();
            _cooldownObserver = DataTracker.GetObservable(data => data.ActiveItemCooldown);
            _cooldownObserver.Subscribe(UpdateAmmoCount);

            _itemObserver = DataTracker.GetObservable(data => data.ActiveItem);
            _itemObserver.Subscribe(UpdateItemRef);
        }

        private void OnDestroy()
        {
            _itemObserver.UnSubscribe(UpdateItemRef);
            _cooldownObserver.UnSubscribe(UpdateAmmoCount);
        }

        public void UpdateItemRef(ActiveData data)
        {
            Icon.sprite = data != null ? data.Sprite : _transparentSprite;
            _maxAmmo = data != null ? data.CooldownSeconds : 0;
            UpdateAmmoCount(data != null ? _cooldownObserver.Value : 0);
        }
    }
}
