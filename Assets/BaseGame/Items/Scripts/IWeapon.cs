using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCPS.SlipForge.Weapon
{
    public interface IWeapon
    {
        public WeaponData Data { get; set; }
        public void AttackBegin();
        public void AttackEnd();
        public void Equip(Transform weaponTransform, Transform projectileTransform, PlayerHand hand);
        public void Drop();
        public void Reload();
    }

}