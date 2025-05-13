using Assets.BaseGame.Guns.Scripts;
using LCPS.SlipForge.Weapon;

namespace LCPS.SlipForge
{
    public static class DataTrackerWeaponUtilities
    {
        public static void EquipLeftWeapon(this DataTracker dt, WeaponData data)
        {
            dt.LeftWeapon.Value = data;
        }

        public static void EquipRightWeapon(this DataTracker dt, WeaponData data)
        {
            dt.RightWeapon.Value = data;
        }

        public static void EquipActiveItem(this DataTracker dt, ActiveData data)
        {
            dt.ActiveItem.Value = data;
        }
    }
}
