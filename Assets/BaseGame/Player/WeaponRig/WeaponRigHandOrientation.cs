using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCPS.SlipForge
{
    public class WeaponRigHandOrientation : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private Transform _cameraTransform;
        // Start is called before the first frame update
        void Start()
        {
            _cameraTransform = Camera.main.transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        
        void Update()
        {
            // The parent forward tells us where the player in aiming in world space
            var forward = transform.parent.forward;

            // The gun is a sprite and should always face the camera, and then rotated about the camera forward axis
            // to appear aligned with the player's forward
            Vector3 gunUp = Vector3.Cross(forward, _cameraTransform.forward);

            transform.LookAt(_cameraTransform, gunUp);
        }
    }
}
