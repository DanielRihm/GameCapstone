using LCPS.SlipForge.Engine;
using LCPS.SlipForge.Weapon;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace LCPS.SlipForge.UI
{
    [RequireComponent(typeof(Button))]
    public class WeaponButton : AbstractDisplayButton
    {
        public bool IsLeftWeapon = false;
        [SerializeField] private InventoryDescription _equipmentDescription;
        private Sprite _transparentSprite;
        private Image _icon;
        private Observable<WeaponData> _weaponObservable;
        protected override void Initialize()
        {
            base.Initialize();

            Assert.IsNotNull(_equipmentDescription, "Equipment Description is not set on Weapon Button");

            // get the icon image
            _icon = GetComponentsInChildren<Image>().Select(x => x).Where(x => x.gameObject.name == "Icon").First();

            // create a transparent sprite for when the player has no weapon equipped
            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, new Color(0, 0, 0, 0));
            texture.Apply();
            _transparentSprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));


            _weaponObservable = DataTracker.GetObservable(data => IsLeftWeapon ? data.LeftWeapon : data.RightWeapon);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _weaponObservable.Subscribe(UpdateWeaponRef);
        }

        protected void OnDisable()
        {
            _weaponObservable.UnSubscribe(UpdateWeaponRef);
        }

        // We want the whole observable object so the Equipment screen can null the value and do what we expect
        public void UpdateWeaponRef(WeaponData data)
        {
            _icon.sprite = data != null ? data.Sprite : _transparentSprite;
        }

        protected override void ViewEvent(bool s = true)
        {
            base.ViewEvent(s);

            // Tell the Inventroy descriptor which weapon to watch.
            _equipmentDescription.SetObservable(_weaponObservable);
        }
    }
}
