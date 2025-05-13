using LCPS.SlipForge;
using LCPS.SlipForge.Weapon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.BaseGame.Items
{
    public static class AbstractWeaponExtensions
    {
        public static void PlaySound(this AbstractWeapon weapon, string soundName)
        {
            if (SoundManager.Instance != null)
            {
                if (weapon.Hand == PlayerHand.Left)
                {
                    SoundManager.Instance.PlayLeftWeapon(soundName);
                }
                else
                {
                    SoundManager.Instance.PlayRightWeapon(soundName);
                }
            }
        }

        public static void StopSound(this AbstractWeapon weapon)
        {
            if (SoundManager.Instance != null)
            {
                if (weapon.Hand == PlayerHand.Left)
                {
                    SoundManager.Instance.StopLeftWeapon();
                }
                else
                {
                    SoundManager.Instance.StopRightWeapon();
                }
            }
        }

        public static void PlaySoundChannelTwo(this AbstractWeapon weapon, string soundName)
        {
            if (SoundManager.Instance != null)
            {
                if (weapon.Hand == PlayerHand.Left)
                {
                    SoundManager.Instance.PlayLeftWeaponCharging(soundName);
                }
                else
                {
                    SoundManager.Instance.PlayRightWeaponCharging(soundName);
                }
            }
        }
    }
}
