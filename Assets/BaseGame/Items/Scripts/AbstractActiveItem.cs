using LCPS.SlipForge;
using LCPS.SlipForge.Weapon;
using System;
using UnityEngine;

namespace Assets.BaseGame.Guns.Scripts
{
    public abstract class AbstractActiveItem : MonoBehaviour
    {
        public ActiveData Data { get; set; }
        protected float _cooldownRemaining
        {
            get => DataTracker.Instance.ActiveItemCooldown.Value;
            set => DataTracker.Instance.ActiveItemCooldown.Value=value;
        }
        [NonSerialized]
        public float AbstractCooldownSeconds;
        [NonSerialized]
        protected float _durationRemaining;
        [NonSerialized]
        public float AbstractDurationSeconds;
        [NonSerialized]
        protected bool isCurrentlyActive;
        protected virtual void Start()
        {
            _cooldownRemaining = AbstractCooldownSeconds;
            isCurrentlyActive = false;
        }
        protected virtual void DoUse()
        {
            isCurrentlyActive = true;
            _cooldownRemaining = AbstractCooldownSeconds;
            _durationRemaining = AbstractDurationSeconds;
            DoEffects();
        }
        protected virtual void FixedUpdate()
        {
            if (_cooldownRemaining > 0)
            {
                _cooldownRemaining -= Time.fixedDeltaTime;
            }
            else if (_cooldownRemaining <= 0)
            {
                _cooldownRemaining = 0;
            }
            if (_durationRemaining > 0)
            {
                _cooldownRemaining = AbstractCooldownSeconds;
                _durationRemaining -= Time.fixedDeltaTime;
            }
            else if (_durationRemaining <= 0 && isCurrentlyActive)
            {
                StopEffects();
                _durationRemaining = 0;
                isCurrentlyActive = false;
            }
        }
        protected virtual void OnUse()
        {
            if (_cooldownRemaining <= 0)
            {
                DoUse();
            }
        }
        protected virtual void DoEffects()
        {
        }

        protected virtual void StopEffects()
        {
        }

		public virtual void Drop()
		{
			Destroy(gameObject);
		}
	}
}
