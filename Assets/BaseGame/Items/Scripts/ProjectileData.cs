using UnityEngine;

namespace LCPS.SlipForge.Weapon
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ProjectileBehavior")]
    public class ProjectileData : ScriptableObject
    {
        public float Size;
        public Sprite Sprite;
        public float Speed;
        public Projectile Projectile;
        public bool Stagger;

    }

}