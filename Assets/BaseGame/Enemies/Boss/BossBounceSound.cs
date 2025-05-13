using LCPS.SlipForge.Enemy.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCPS.SlipForge.Enemy
{
    public class BossBounceSound : MonoBehaviour
    {
        public AudioClip BounceSound;

        public MovementSetSeekBehavior Movement;

        private Vector3 LastDirection;

        // Start is called before the first frame update
        public void Start()
        {
            LastDirection = Movement.Direction;
            SoundManager.Instance.RegisterSFX(BounceSound.name, BounceSound);
        }

        // Update is called once per frame
        void Update()
        {
            if (Mathf.Sign(Movement.Direction.x) != Mathf.Sign(LastDirection.x))
            {
                PlayBounceSound();
            }
            if (Mathf.Sign(Movement.Direction.z) != Mathf.Sign(LastDirection.z))
            {
                PlayBounceSound();
            }

            LastDirection = Movement.Direction;
        }

        private void PlayBounceSound()
        {
            if(LastDirection == Vector3.zero)
            {
                return;
            }

            SoundManager.Instance.PlaySFX(BounceSound.name);
        }
    }
}
