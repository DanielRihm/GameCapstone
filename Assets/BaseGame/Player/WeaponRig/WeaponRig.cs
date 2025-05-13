using Assets.BaseGame.Guns.Scripts;
using Assets.BaseGame.Scripts.Engine.Data;
using LCPS.SlipForge.Player;
using LCPS.SlipForge.Util;
using LCPS.SlipForge.Weapon;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge
{
    public class WeaponRig : MonoBehaviour, IWeaponRig
    {
        public static IWeaponRig Instance;
        private Transform _leftHandTransform;
        private Transform _rightHandTransform;
        private Transform _leftProjectileTransform;
        private Transform _rightProjectileTransform;

        private IWeapon _leftWeapon;
        private IWeapon _rightWeapon;

        private AbstractActiveItem _activeItem;

        private WeaponRigForward _forward;

        public ItemPickup _itemPickup;
        public ActivePickup _activePickup;

        public Vector3 Forward => _forward != null && _forward.transform != null ? _forward.transform.forward : Vector3.zero;
        private DataTracker _DT => DataTracker.Instance;
        private PlayerInstance _playerInstance => PlayerInstance.Instance;

        public WeaponData _dummyData;

        private void Start()
        {
            Instance = this;
            _leftHandTransform = transform.Find("Root/Hand.Left");
            _rightHandTransform = transform.Find("Root/Hand.Right");
            _leftProjectileTransform = transform.Find("Root/Projectile.Left");
            _rightProjectileTransform = transform.Find("Root/Projectile.Right");
            _forward = GetComponentInChildren<WeaponRigForward>();

            Assert.IsNotNull(_leftHandTransform, "Left hand transform not found");
            Assert.IsNotNull(_rightHandTransform, "Right hand transform not found");
            Assert.IsNotNull(_leftProjectileTransform, "Left projectile transform not found");
            Assert.IsNotNull(_rightProjectileTransform, "Right projectile transform not found");
            Assert.IsNotNull(_forward, "WeaponRigForward not found");
            Assert.IsNotNull(_DT, "DataTracker not found");
            Assert.IsNotNull(_playerInstance, "PlayerInstance not found");

            Init();
        }

        private void Init()
        {
            DataTracker.Subscribe(data => data.LeftWeapon, LeftWeapon_OnValueChanged);
            DataTracker.Subscribe(data => data.RightWeapon, RightWeapon_OnValueChanged);
            DataTracker.Subscribe(data => data.ActiveItem, ActiveItem_OnValueChanged);
        }

        public void OnDestroy()
        {
            DataTracker.UnSubscribe(data => data.LeftWeapon, LeftWeapon_OnValueChanged);
            DataTracker.UnSubscribe(data => data.RightWeapon, RightWeapon_OnValueChanged);
            DataTracker.UnSubscribe(data => data.ActiveItem, ActiveItem_OnValueChanged);
        }

        private void LeftWeapon_OnValueChanged(WeaponData weapon)
        {
            if (weapon == null)
            {
                UnequipLeftWeapon();
                return;
            }

            EquipLeftHand(weapon);
        }   

        private void RightWeapon_OnValueChanged(WeaponData weapon)
        {
            if (weapon == null)
            {
                UnequipRightWeapon();
                return;
            }

            EquipRightHand(weapon);
        }

        private void ActiveItem_OnValueChanged(ActiveData item)
        {
            if (item == null)
            {
                UnequipActive();
                return;
            }

            EquipActive(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">NOT NULL</param>
        /// <returns></returns>
        public bool EquipActivePickup(ActiveData data)
        {
            if (_activeItem == null)
            {
                _playerInstance.PickUpActive(data);
                return true;
            }

            return false;
        }

        private void EquipActive(ActiveData data)
        {
            var activeItem = Instantiate(data.Prefab)
                .SetData(data);

            // Parent the item to the player
            activeItem.transform.parent = this.transform;
            // Reset the weapon orientation
            activeItem.transform.localRotation = Quaternion.identity;

            _activeItem = activeItem;
        }

        private ActiveData UnequipActive()
        {

            // Nothing to do
            if (_activeItem == null)
            {
                return null;
            }

            var data = _activeItem.Data;
            _activeItem.Drop(); // TODO: weapons should probalby use Destory to clean up
            Destroy(((AbstractActiveItem)_activeItem).gameObject);
            _activeItem = null;

            //DropActive(data, new Vector3(0, 0.9f, 2));

            return data;
        }

        private void Update()
        {
            // Projectile transfroms need to respond to WeaponData.
            if (_rightWeapon != null)
            {
                _rightProjectileTransform.localPosition = _rightWeapon.Data.ProjectileOffset;
            }
            if (_leftWeapon != null)
            {
                // The left hand needs to inverst the x axis, Copy to avoid modifying the original data
                var leftOffset = _leftWeapon.Data.ProjectileOffset.Copy().InvertX();
                _leftProjectileTransform.localPosition = leftOffset;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">NOT NULL</param>
        /// <returns></returns>
        public bool Equip(WeaponData data)
        {
            // TODO: Two handed weapons
            if (data.IsHeavy)
            {
                if (_rightWeapon == null && _leftWeapon == null)
                {
                    _playerInstance.PickUpWeapon(data, PlayerHand.Right);
                    _playerInstance.PickUpWeapon(_dummyData, PlayerHand.Left);
                    return true;
                }
            }

            // TODO: Which hand to equip to? Equipping to the empty hand for now
            else if (_rightWeapon == null)
            {
                _playerInstance.PickUpWeapon(data, PlayerHand.Right);
                return true;
            }
            else if (_leftWeapon == null)
            {
                _playerInstance.PickUpWeapon(data, PlayerHand.Left);
                return true;
            }

            return false;
        }

        public IWeapon EquipLeftHand(WeaponData data)
        {
            if (_leftWeapon != null)
            {
                Unequip(_leftWeapon);
            }
            
            var weapon = Instantiate(data.Prefab)
                .SetData(data);

            weapon.Equip(_leftHandTransform, _leftProjectileTransform, PlayerHand.Left);
            _leftWeapon = weapon;

/*            if (_leftWeapon.Data.IsHeavy)
            {
                EquipRightHand(_dummyData);
            }*/

            return weapon;
        }

        public IWeapon EquipRightHand(WeaponData data)
        {
            if (_rightWeapon != null)
            {
                Unequip(_rightWeapon);
            }

            var weapon = Instantiate(data.Prefab)
                .SetData(data);

            weapon.Equip(_rightHandTransform, _rightProjectileTransform, PlayerHand.Right);
            _rightWeapon = weapon;

/*            if (_rightWeapon.Data.IsHeavy)
            {
                EquipLeftHand(_dummyData);
            }*/

            return weapon;
        }

        public WeaponData Unequip(IWeapon weapon)
        {
            WeaponData oldWeapon = null;
            var h = (weapon as AbstractWeapon).Hand;

            if (h == PlayerHand.Right)
            {
                oldWeapon = UnequipRightWeapon();
            }
            else if (h == PlayerHand.Left)
            {
                oldWeapon = UnequipLeftWeapon();
			}

            return oldWeapon;
        }

        private WeaponData UnequipRightWeapon()
        {
            // Nothing to do
            if (_rightWeapon == null)
            {
                return null;
            }

            var data = _rightWeapon.Data;
            _rightWeapon.Drop(); // TODO: weapons should probalby use Destory to clean up
            Destroy(((AbstractWeapon)_rightWeapon).gameObject);
            _rightWeapon = null;

            if (data.IsHeavy || data == _dummyData)
            {
                UnequipLeftWeapon();
            }

            return data;
        }

        private WeaponData UnequipLeftWeapon()
        {
            // Nothing to do
            if(_leftWeapon == null)
            {
                return null;
            }

            var data = _leftWeapon.Data;
            _leftWeapon.Drop(); // TODO: weapons should probalby use Destory to clean up
            Destroy(((AbstractWeapon)_leftWeapon).gameObject);
            _leftWeapon = null;

            if (data.IsHeavy || data == _dummyData)
            {
                UnequipRightWeapon();
            }

            return data;
        }

        // This class is getting a littel bloated
        public void OnAttack1(bool isPressed)
        {
            if (_rightWeapon != null)
            {
                if (isPressed)
                {
                    _rightWeapon.AttackBegin();
                }
                else
                {
                    _rightWeapon.AttackEnd();
                }
            }
        }

        public void OnAttack2(bool isPressed)
        {
            if (_leftWeapon != null)
            {
                if (isPressed)
                {
                    _leftWeapon.AttackBegin();
                }
                else
                {
                    _leftWeapon.AttackEnd();
                }
            }
        }

        public Transform GetAbilityOrigin()
        {
            return transform;
        }

        public void OnReload()
        {
            if (_rightWeapon != null) _rightWeapon.Reload();
            if (_leftWeapon != null) _leftWeapon.Reload();
        }

        public void CreateWeaponPickup(WeaponData data, Vector3 dropOffset)
        {
            // Need to parent the drop to something, might as well be the player parent
            if (data == _dummyData) return; //Don't make a pickup for the dummy
            var drop = Instantiate(_itemPickup, transform.position + dropOffset, Quaternion.Euler(45, 0, 0), _playerInstance.transform.parent);
            drop.Pickup = data;
        }

        public void CreateActivePickup(ActiveData data, Vector3 dropOffset)
        {
            // Need to parent the drop to something, might as well be the player parent
            var drop = Instantiate(_activePickup, transform.position + dropOffset, Quaternion.Euler(45, 0, 0), _playerInstance.transform.parent);
            drop.Pickup = data;
        }
    }
}
