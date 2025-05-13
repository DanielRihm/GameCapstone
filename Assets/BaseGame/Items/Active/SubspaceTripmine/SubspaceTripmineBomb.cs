using Cinemachine;
using LCPS.SlipForge.Enemy;
using UnityEngine;

namespace Assets.BaseGame.Items.Active.SubspaceTripmine
{
    public class SubspaceTripmineBomb : MonoBehaviour
    {
        public float BaseDamage;
        public float ArmSeconds;
        public float DetectRadius;
        public float DamageRadius;

        private float _currentTime;
        private bool _isArmed;
        private CinemachineImpulseSource _impulseSource;

        public void Start()
        {
            _impulseSource = GetComponent<CinemachineImpulseSource>();
            transform.forward = Camera.main.transform.forward;
            _currentTime = 0;
            _isArmed = false;
        }
        public void InitializeStats(float damage, float armSec, float detectRad, float damageRad)
        {
            BaseDamage = damage;
            ArmSeconds = armSec;
            DetectRadius = detectRad;
            GetComponent<SphereCollider>().radius = DetectRadius;
            DamageRadius = damageRad;
        }

        public void FixedUpdate()
        { 
            if (_currentTime >= ArmSeconds)
            {
                _isArmed = true;
                GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.5f);
            }
            else
            {
                _currentTime += Time.fixedDeltaTime;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isArmed) return;
            if (other.gameObject.TryGetComponent<Enemy>(out var component))
            {
                foreach(Collider c in Physics.OverlapSphere(transform.position, DamageRadius))
                {
                    if (c.gameObject.TryGetComponent<Enemy>(out var component2)) component2.TakeDamage((int)BaseDamage);
                }
                Destroy(this.gameObject);
            }
        }

        private void OnDestroy()
        {
            _impulseSource.GenerateImpulse();
        }
    }
}
