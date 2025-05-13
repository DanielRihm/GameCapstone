
using LCPS.SlipForge.Weapon;
using UnityEngine;

namespace LCPS.SlipForge.Player
{
    public interface IWeaponRig
    {
        public Vector3 Forward { get; }
        IWeapon EquipRightHand(WeaponData data);
        IWeapon EquipLeftHand(WeaponData data);
        bool Equip(WeaponData data);
        void CreateWeaponPickup(WeaponData weapon, Vector3 offset);
    }
}