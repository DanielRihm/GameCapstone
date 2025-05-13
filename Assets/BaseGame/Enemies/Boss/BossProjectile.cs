using LCPS.SlipForge.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace LCPS.SlipForge
{
    public class BossProjectile : MonoBehaviour
    {
        public float Speed = 1.0f;
        public Vector3 Forward;
        public int MaxBounces = 1;
        private int _bounces;

        public AudioClip RedirectClip;
        public float DarmTime = 1.0f;
        private bool isArmed;


        // Start is called before the first frame update
        void Start()
        {
            _bounces = MaxBounces;
            Forward = Forward.normalized;

            if(RedirectClip != null)
            {
                SoundManager.Instance.RegisterSFX(RedirectClip.name, RedirectClip);
            }

            StartCoroutine(Arm());
        }

        private IEnumerator Arm()
        {
            yield return new WaitForSeconds(DarmTime);
            isArmed = true;
        }

        // Update is called once per frame
        void Update()
        {
            // Refelect movement off the edge of the nav mesh
            if (NavMesh.Raycast(transform.position, transform.position + Forward, out var hit, NavMesh.AllAreas))
            {
                _bounces--;
                var reflect = Vector3.Reflect(Forward, hit.normal);
                Forward = reflect.normalized;
            }

            transform.position += Forward * Speed * Time.deltaTime;

            if (_bounces < 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isArmed && other.tag == "Boss" && PlayerInstance.Instance is PlayerInstance player)
            {
                // send the projectile at the player
                
                var position = transform.position;
                
                // Why is it so hard to keep y values out of here.
                position.y = 0;

                var direction = player.transform.position - transform.position;
                direction.y = 0;

                if(RedirectClip != null)
                {
                    SoundManager.Instance.PlaySFX(RedirectClip.name);
                }

                Forward = direction.normalized * 2;
            }
        }
    }
}
