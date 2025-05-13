using LCPS.SlipForge.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge
{
    public class BehaviorDeath : BehaviorBase
    {
        [SerializeField] private GameObject _deathFX;
        [SerializeField] private ChipPickup _chipPickup;

        void Start()
        {
            Assert.IsNotNull(_chipPickup, "ChipPickup is null.");
        }

        void OnEnable()
        {
            // Give things a chance to happen on death.
            if (this.gameObject == null) return;

            Die();
        }

        // Update is called once per frame
        void OnDisable()
        {
            // Do not prevent death
            //StopAllCoroutines();

        }

        private void Die()
        {
            if(_deathFX != null)
            {
                Instantiate(_deathFX, transform.position, Quaternion.identity, transform.parent);
            }

            if (DataTracker.Instance.IsDungeon.Value)
            {
                var _parent = this.GetComponentInParent<DungeonFloor>();
                Assert.IsNotNull(_parent, "Parent of enemy is not a dungeon floor");
                DungeonManager.Instance.EnemyKilled(_parent.Location);
            }

            var chipsDrop = Instantiate(_chipPickup, transform.position + Vector3.up, Quaternion.Euler(45, 0, 0), transform.parent);

            Destroy(gameObject, 1 / 60f);
        }
    }

}