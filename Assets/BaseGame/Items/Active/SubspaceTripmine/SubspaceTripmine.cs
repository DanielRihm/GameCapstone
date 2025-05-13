using Assets.BaseGame.Items.Active.SubspaceTripmine;
using LCPS.SlipForge;
using UnityEngine;

namespace Assets.BaseGame.Guns.Scripts
{
    public class SubspaceTripmine : AbstractActiveItem
    {
        public float BaseDamage = 400f;
        public float ArmSeconds = 2;
        public float DetectRadius = 1;
        public float DamageRadius = 6;

        public SubspaceTripmineBomb Bomb;

        protected override void Start()
        {
            AbstractCooldownSeconds = Data.CooldownSeconds;
            AbstractDurationSeconds = Data.DurationSeconds;
            base.Start();
        }

        protected override void DoEffects()
        {
            var origin = PlayerInstance.Instance.transform;
            var proj = Instantiate(Bomb, origin.position + Vector3.up, origin.rotation, PlayerInstance.Instance.transform.parent);
            proj.InitializeStats(BaseDamage, ArmSeconds, DetectRadius, DamageRadius);
            SoundManager.Instance.PlaySFX("place_mine_sound");
        }

    }
}