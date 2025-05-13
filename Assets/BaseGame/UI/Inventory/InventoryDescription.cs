using LCPS.SlipForge.Engine;
using LCPS.SlipForge.Weapon;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace LCPS.SlipForge.UI
{
    public class InventoryDescription : MonoBehaviour
    {

        [SerializeField] private TMP_Text WeaponName;
        [SerializeField] private TMP_Text WeaponDesc;
        [SerializeField] private Image WeaponSprite;
        public Button DropButton;
        private Observable<WeaponData> _weapon = new();
        private Observable<ActiveData> _active = new();

        void OnEnable()
        {
            DropButton.onClick.AddListener(Drop);
        }

        void OnDisable()
        {
            // Preavent leaking subscribers
            DropButton.onClick.RemoveAllListeners();
        }

        public void SetObservable(Observable<WeaponData> value)
        {
            UnSubscribeReferences();
            _weapon = value;
            _weapon.Subscribe(UpdateWeaponRef<WeaponData>);
            _active = null;
        }

        public void SetObservable(Observable<ActiveData> value)
        {
            UnSubscribeReferences();
            _active = value;
            _active.Subscribe(UpdateWeaponRef<ActiveData>);
            _weapon = null;
        }

        private void UnSubscribeReferences()
        {
            _weapon?.UnSubscribe(UpdateWeaponRef<WeaponData>);
            _active?.UnSubscribe(UpdateWeaponRef<ActiveData>);
        }

        private void UpdateWeaponRef<T>(T data)
        {
            Assert.IsTrue(typeof(T) == typeof(WeaponData) || typeof(T) == typeof(ActiveData), "Data is not of type WeaponData or ActiveData");

            if (data == null)
                SetNullWeaponVariables();
            else if (data is WeaponData weapon)
                SetWeaponVariables(weapon);
            else if (data is ActiveData active)
                SetActiveVariables(active);
            else
                Debug.LogWarning("Data is not of type WeaponData or ActiveData");
        }


        private void SetWeaponVariables(WeaponData weapon)
        {
            WeaponName.text = weapon.Name;
            WeaponDesc.text = weapon.Description;
            WeaponSprite.sprite = weapon.Sprite;
            WeaponSprite.enabled = true;
            DropButton.interactable = true;
        }

        private void SetActiveVariables(ActiveData active)
        {
            WeaponName.text = active.Name;
            WeaponDesc.text = active.Description;
            WeaponSprite.sprite = active.Sprite;
            WeaponSprite.enabled = true;
            DropButton.interactable = true;
        }

        private void SetNullWeaponVariables()
        {
            WeaponName.text = "No Weapon";
            WeaponDesc.text = "You have no weapon equipped in this slot.";
            WeaponSprite.enabled = false;
            DropButton.interactable = false;
        }

        private void Drop()
        {
            if (_weapon != null)
                DropWeapon();
            else if (_active != null)
                DropActive();
        }

        private void DropWeapon()
        {
            PlayerInstance.Instance.Drop(_weapon);
        }

        private void DropActive()
        {
            PlayerInstance.Instance.Drop(_active.Value, new Vector3(0, 1, 1.25f));
            _active.Value = null;
        }
    }
}