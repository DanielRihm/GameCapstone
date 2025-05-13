using System.Collections.Generic;
using UnityEngine;

namespace LCPS.SlipForge.Weapon
{
	[CreateAssetMenu(menuName = "ScriptableObjects/WeaponBehavior")]
	public class WeaponData : ScriptableObject
	{
		public string Name;
		public string Description;
		public bool IsHeavy;
		[Range(0, 60)]
		public float FireRate;
		[Tooltip("Time in seconds accept an attack input if too early")]
		public float AttackGracePeriod = 0; // Tollerance to clicking too early to attack
		public Sprite Sprite;
		public string FireSound;
		[Range(0, 500)]
		public int BaseDamage = 1;
		public AbstractWeapon Prefab;
		public Vector3 HandOffset;
		public Vector3 ProjectileOffset;
		public Projectile Projectile;
		public bool IsAutomatic;
		[Range(0, 100)]
		public int ProjectileCount;
		[Range(0, 360)]
		public float SpreadAngle;
		[Range(-1, 200)] //-1 means ammo-less weapon
		public int AmmoCount;
		public int ReloadTime;
		public string ReloadSound;
        public List<WeaponFeedback> Feedback;
		[Tooltip("Magnatude of camera shake from recoil.")]
		[Range(0, 1)]
		public float KickbackScalar = 0;
		[Range(1, 5)]
		public int Tier = 1;
    }

}