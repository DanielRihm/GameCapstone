using Assets.BaseGame.Items;
using LCPS.SlipForge;
using LCPS.SlipForge.Engine;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Weapon
{

    public class LaserWeapon : AbstractWeapon
    {
        public AudioClip WarmupSound;
        public AudioClip SustainSound;
        public AudioClip CooldownSound;
        public float RechageRate = 1;

        private Projectile _laserBeam;

        private Coroutine _soundRoutine;

        private float _ammoCountFloat;
        private Observable<int> _ammoCount;
        private Observable<bool> _reloading;

        // Start is called before the first frame update
        void Start()
        {
            Assert.IsNotNull(Data, $"{name} WeaponData is null");

            if(SoundManager.Instance != null)
            {
                SoundManager.Instance.RegisterSFX(WarmupSound.name, WarmupSound);
                SoundManager.Instance.RegisterSFX(SustainSound.name, SustainSound);
                SoundManager.Instance.RegisterSFX(CooldownSound.name, CooldownSound);
            }

            if(this.Hand == PlayerHand.Left)
            {
                _ammoCount = DataTracker.GetObservable(d => d.LeftWeaponAmmoCount);
                _reloading = DataTracker.GetObservable(d => d.LeftWeaponReloading);
            }
            else
            {
                _ammoCount = DataTracker.GetObservable(d => d.RightWeaponAmmoCount);
                _reloading = DataTracker.GetObservable(d => d.RightWeaponReloading);
            }

            _ammoCountFloat = 0;
        }

        private void Update()
        {
            // The laser automatically recarges when not shooting
            if (_soundRoutine == null)
            {
                _ammoCountFloat = Mathf.Clamp(_ammoCountFloat + RechageRate * Time.deltaTime, 0, 100);
            }

            _reloading.Value = _soundRoutine == null;
            _ammoCount.Value = (int)_ammoCountFloat;
        }

        public override void AttackBegin()
        {
            if (_ammoCount.Value < 1)
                return;

            _laserBeam = Instantiate(Data.Projectile, ProjectileTransform);
            _laserBeam.Damage = Data.BaseDamage;
            
            PlayFeedback(true);
            if(_soundRoutine != null)
                StopCoroutine(_soundRoutine);

            _soundRoutine = StartCoroutine(PlaySoundRoutine());
        }

        private void StopAll()
        {
            if (_soundRoutine != null)
            {
                StopCoroutine(_soundRoutine);
                _soundRoutine = null;
            }
            if (this.Hand== PlayerHand.Right)
            {
                SoundManager.Instance.StopRightWeaponLoop();
            }
            else
            {
                SoundManager.Instance.StopLeftWeaponLoop();
            }

            StopFeedback();
        }

        private IEnumerator PlaySoundRoutine()
        {
            yield return new WaitForEndOfFrame();

            if (this.Hand == PlayerHand.Right)
            {
                SoundManager.Instance.PlayRightWeaponLoop(SustainSound.name);
            }
            else
            {
                SoundManager.Instance.PlayLeftWeaponLoop(SustainSound.name);
            }

            while(_ammoCountFloat > 0)
            {
                _ammoCountFloat = Mathf.Max(_ammoCountFloat - RechageRate * Time.deltaTime, 0);
                yield return null;
            }

            AttackEnd();
            _soundRoutine = null;
        }

        public override void AttackEnd()
        {
            if (this.Hand == PlayerHand.Right)
            {
                SoundManager.Instance.StopRightWeaponLoop();

            }
            else
            {
                SoundManager.Instance.StopLeftWeaponLoop();
            }


            if(_laserBeam != null)
                Destroy(_laserBeam.gameObject);

            StopAll();
        }

        public override void Reload()
        {
        }
    }

}