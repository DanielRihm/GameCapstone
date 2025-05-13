using Assets.BaseGame.Guns.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace LCPS.SlipForge.Weapon
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ActiveItemData")]
    public class ActiveData : ScriptableObject
    {
        public string Name;
        public string Description;
        public Sprite Sprite;
        public string UseSound;
        public AbstractActiveItem Prefab;
        [Range(0, 180)]
        public float CooldownSeconds;
        [Range(0, 180)]
        public float DurationSeconds;
        [Range(1, 5)]
        public int Tier = 1;
    }

}