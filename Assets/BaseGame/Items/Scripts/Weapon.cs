using LCPS.SlipForge.Engine;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Weapon
{

    public class Weapon : AbstractWeapon
    {
        [SerializeField] AudioClip WeaponAudioClip;
        [SerializeField] AudioClip WeaponEmptyAudioClip;
        [SerializeField] AudioClip ReloadAudioClip;

        public int CurrentAmmo
        {
            get => Hand == PlayerHand.Right ? DataTracker.Instance.RightWeaponAmmoCount.Value : DataTracker.Instance.LeftWeaponAmmoCount.Value;
            set
            {
                if (Hand == PlayerHand.Right)
                {
                    DataTracker.Instance.RightWeaponAmmoCount.Value = value;
                }
                else if (Hand == PlayerHand.Left)
                {
                    DataTracker.Instance.LeftWeaponAmmoCount.Value = value;
                }
            }
        }
        protected float _secondsRemaining;
        private float _secondsTotal;

        public float ReloadTimer;
        public bool IsReloading
        {
            get => Hand == PlayerHand.Right ? DataTracker.Instance.RightWeaponReloading.Value : DataTracker.Instance.LeftWeaponReloading.Value;
            set
            {
                if (Hand == PlayerHand.Right)
                {
                    DataTracker.Instance.RightWeaponReloading.Value = value;
                }
                else if (Hand == PlayerHand.Left)
                {
                    DataTracker.Instance.LeftWeaponReloading.Value = value;
                }
            }
        }

        private bool _isShooting;
        protected Coroutine _shootCoroutine;

        // Start is called before the first frame update
        void Start()
        {

            CurrentAmmo = Data.AmmoCount == -1 ? -1 : 0;

            Debug.Log(WeaponAudioClip.name);

            Assert.IsNotNull(Data, $"{name} WeaponData is null");
            if (SoundManager.Instance != null)
            {
                Assert.IsNotNull(WeaponAudioClip, $"{name} AudioClip is null");
                SoundManager.Instance.RegisterSFX(WeaponAudioClip.name, WeaponAudioClip);
                SoundManager.Instance.RegisterSFX(WeaponEmptyAudioClip.name, WeaponEmptyAudioClip);
                SoundManager.Instance.RegisterSFX(ReloadAudioClip.name, ReloadAudioClip);
            }

            _secondsTotal = 1f / Data.FireRate;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            _secondsRemaining -= Time.fixedDeltaTime;

            //NOTE: Grace period does apply to automatics, can feel weird if it's >0
            if (_secondsRemaining < Data.AttackGracePeriod && Data.IsAutomatic && _isShooting && _shootCoroutine == null)
            {
                _shootCoroutine = StartCoroutine(QueueShoot());
            }
            
            if (IsReloading)
            {
                if (ReloadTimer > 0) { ReloadTimer -= Time.fixedDeltaTime; }
                else 
                {
                    CurrentAmmo = Data.AmmoCount;
                    IsReloading = false;
                }
            }
        }

        public override void AttackBegin()
        {
            if (CurrentAmmo == 0)
            {
                if (this.Hand == PlayerHand.Right) SoundManager.Instance.PlayRightWeapon(WeaponEmptyAudioClip.name);
                else SoundManager.Instance.PlayLeftWeapon(WeaponEmptyAudioClip.name);
            }
            else
            {
                // Buffer the attack click if it was close enough.
                if (Data.IsAutomatic) _isShooting = true;
                else if (_secondsRemaining < Data.AttackGracePeriod && _shootCoroutine == null)
                {
                    _shootCoroutine = StartCoroutine(QueueShoot());
                }
            }
        }

        public IEnumerator QueueShoot()
        {
            yield return new WaitForSeconds(_secondsRemaining);

            DoShoot();

            _shootCoroutine = null;
        }

        protected void DoShoot()
        {
            if (CurrentAmmo == 0)
            {
                return;
            }

            else if (CurrentAmmo > 0) CurrentAmmo--;

            _secondsRemaining = _secondsTotal;

            if(this.Hand == PlayerHand.Right) SoundManager.Instance.PlayRightWeapon(WeaponAudioClip.name);
            else SoundManager.Instance.PlayLeftWeapon(WeaponAudioClip.name);

            for(int i = 0; i < Data.ProjectileCount; i++)
            {
                var proj = Instantiate(Data.Projectile, ProjectileTransform.position, ProjectileTransform.rotation);

                // TODO: Parent bullets to some scene root so uload cleans up the bullets

                proj.Damage = Data.BaseDamage; //Todo: put damage multiplier here

                proj.transform.Rotate(Vector3.up, Random.Range(-Data.SpreadAngle / 2f, Data.SpreadAngle / 2f));
                proj.Direction = proj.transform.forward * proj.Speed;
                proj.transform.forward = Camera.main.transform.forward;
                proj.transform.Rotate(new Vector3(0, 0, -this.gameObject.transform.eulerAngles.z + 180));
            }

			// Run the feedback
			PlayFeedback(false);
        }


        public override void AttackEnd()
        {
            if (Data.IsAutomatic) _isShooting = false;
        }

        public override void Reload()
        {
            if ( CurrentAmmo == Data.AmmoCount || IsReloading) return;

            else if ( CurrentAmmo >= 0)
            {
                if (this.Hand == PlayerHand.Right) SoundManager.Instance.PlayRightWeapon(ReloadAudioClip.name);
                else SoundManager.Instance.PlayLeftWeapon(ReloadAudioClip.name);
                ReloadTimer = Data.ReloadTime;
                IsReloading = true;
                CurrentAmmo = 0;
            }
        }
    }

}