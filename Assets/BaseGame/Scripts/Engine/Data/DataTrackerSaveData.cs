using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.BaseGame.Guns.Scripts;
using LCPS.SlipForge.Engine;
using LCPS.SlipForge.Weapon;
using UnityEngine;

namespace Assets.BaseGame.Scripts.Engine.Data
{
    [Serializable]
    public class DataTrackerSaveData : ISaveGameData
    {
        public Observable<int> HP = new(3);
        public Weapons Weapons = new();
        public Currency Currency = new();
        public Stats Stats = new();
    }

    [Serializable]
    public class Weapons
    {
        public Observable<WeaponData> LeftWeapon = new();
        public Observable<WeaponData> RightWeapon = new();
        public Observable<ActiveData> ActiveItem = new();
    }

    [Serializable]
    public class Currency
    {
        public Observable<int> PotatoChips = new();
    }

    [Serializable]
    public class Stats
    {
        public Observable<int> MaxHP = new(3);
        public Observable<float> WalkSpeedMultiplier = new();
        public Observable<float> DodgeSpeedMultipler = new();
    }
}
