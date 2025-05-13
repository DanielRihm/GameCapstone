using Assets.BaseGame.Guns.Scripts;
using UnityEngine;

namespace LCPS.SlipForge.Weapon
{
    public static class MonoBehaviourExtensions
    {
        public static AbstractWeapon SetData(this AbstractWeapon weapon, WeaponData data)
        {
            weapon.Data = data;
            return weapon;
        }

        public static AbstractActiveItem SetData(this AbstractActiveItem item, ActiveData data)
        {
            item.Data = data;
            return item;
        }
    } 
}