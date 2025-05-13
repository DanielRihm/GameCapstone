using Cinemachine;
using System.Collections;
using UnityEngine;

namespace LCPS.SlipForge.Weapon
{
    public abstract class AbstractWeapon : MonoBehaviour, IWeapon
    {
        public WeaponData Data { get; set; }
        public Transform ProjectileTransform;
        public PlayerHand Hand;
        public ItemPickup ItemPickup;
        

        private Coroutine _feedbackRoutione;
        private SpriteRenderer _sr;
        private CinemachineImpulseSource _impulseSource;

        public abstract void AttackBegin();
        public abstract void AttackEnd();

        public void Equip(Transform weaponTransform, Transform projectileTransform, PlayerHand h)
        {
            transform.SetParent(weaponTransform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            Hand = h;

            ProjectileTransform = projectileTransform;

            // Default behavior is to flip the sprite for the other hand.
            _sr = GetComponent<SpriteRenderer>();
            if (_sr != null)
            {
                StartCoroutine(KeepWeaponUpright());
            }
        }

        private IEnumerator KeepWeaponUpright()
        {
            while (true)
            {
                if(transform.right.x > 0)
                {
                    _sr.flipY = true;
                }
                else
                {
                    _sr.flipY = false;
                }

                yield return null;
            }
        }

        protected void PlayFeedback(bool loop)
        {
            _feedbackRoutione = StartCoroutine(PlayFeedbackRoutine(loop));
        }

        protected void StopFeedback()
        {
            if (_feedbackRoutione != null)
                StopCoroutine(_feedbackRoutione);

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        private IEnumerator PlayFeedbackRoutine(bool loop)
        {
            if (Data == null || Data.Feedback == null || Data.FireRate == 0f)
            {
                yield break;
            }

            // Prevent double feedback
            if (_feedbackRoutione != null) {
                StopCoroutine(_feedbackRoutione);
            }

            if(_impulseSource == null)
            {
                _impulseSource = GetComponent<CinemachineImpulseSource>();
            }
            
            if(_impulseSource != null)
            {
                // Forward is at the camera, right is "forward" for the gun sprite.
                _impulseSource.GenerateImpulse(transform.right * Data.KickbackScalar);
            }

            var duration = 1f / Data.FireRate;
            float startTime = Time.time;
            float elapsedTime = 0f;
            while (loop || elapsedTime < duration)
            {
                elapsedTime = Time.time - startTime;
                var position = Vector3.zero;
                var rotationRadians = 0f;
                foreach (var feedback in Data.Feedback)
                {
                    if (feedback == null)
                    {
                        continue;
                    }
                    position += feedback.EvaluatePosition(elapsedTime, duration);
                    rotationRadians += feedback.EvaluateRotation(elapsedTime, duration);
                }

                // Invert the y axis when in the left hand
                if (Hand == PlayerHand.Left)
                {
                    position = new Vector3(position.x, -position.y, position.z);
                    rotationRadians = -rotationRadians;
                }

                transform.localPosition = position;
                var rotation = rotationRadians * Mathf.Rad2Deg;
                transform.localRotation = Quaternion.Euler(0, 0, rotation);

                yield return null;
            }

            ResetTransform();
        }

        private void ResetTransform()
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public virtual void Drop()
        {
            Destroy(gameObject);
        }

        public abstract void Reload();
    }
}

