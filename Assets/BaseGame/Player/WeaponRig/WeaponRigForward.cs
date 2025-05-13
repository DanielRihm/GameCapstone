using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace LCPS.SlipForge
{
    public class WeaponRigForward : MonoBehaviour
    {
        [SerializeField]
        private Vector3 CursorPlane;

        private ActionMap _actionMap;

        // Start is called before the first frame update
        void Start()
        {
            _actionMap = new ActionMap();
            _actionMap.Enable();

            Assert.IsNotNull(_actionMap, "Weapon Rig could not find ActionMap");        
        }

        // Update is called once per frame
        void Update()
        {
            // Get the AIM vector
            var aimInput = _actionMap.Player.JoystickAim.ReadValue<Vector2>();
            var aimDirection = new Vector3(aimInput.x, 0, aimInput.y);

            // Normalize aim to be player relative if input is coming from a mouse.
            if (aimInput.sqrMagnitude == 0)
            {
                var mouseInput = _actionMap.Player.MouseAim.ReadValue<Vector2>();
                var worldPlane = new Plane(CursorPlane.normalized, 0);
                Ray ray = Camera.main.ScreenPointToRay(mouseInput);
                if (worldPlane.Raycast(ray, out float distance))
                {
                    Vector3 target = ray.GetPoint(distance);

                    aimDirection = (target - transform.position).normalized;
                }

            }
            aimDirection.y = 0;
            transform.LookAt(transform.position + aimDirection, Vector3.up);
        }
    }
}
