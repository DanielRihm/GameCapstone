using LCPS.SlipForge.Engine;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Weapon
{

    public class AutomaticWeapon : Weapon
    {
        private bool _isShooting;
        // Update is called once per frame
        void FixedUpdate()
        {
            _secondsRemaining -= Time.fixedDeltaTime;

            if (_isShooting) QueueShoot();
            
            if (IsReloading)
            {
                if (ReloadTimer > 0) { ReloadTimer -= Time.fixedDeltaTime; }
                else 
                {
                    IsReloading = false;
                    CurrentAmmo = Data.AmmoCount;
                }
            }
        }

        public override void AttackBegin()
        {
            // Buffer the attack click if it was close enough.
            if (_secondsRemaining < Data.AttackGracePeriod && _shootCoroutine == null)
            {
                //_shootCoroutine = StartCoroutine(QueueShoot());
                _isShooting = true;
            }
        }

        public override void AttackEnd()
        {
            _isShooting = false;
        }
    }

}