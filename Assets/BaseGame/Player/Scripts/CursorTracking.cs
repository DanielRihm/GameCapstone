using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge
{
    public class CursorTracking : MonoBehaviour
    {
        public Transform CrosshairPos;

        [SerializeField]
        private Vector3 CursorPlane;

        [SerializeField]
        private float joystickAimDistance;

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
            var mouseInput = _actionMap.Player.MouseAim.ReadValue<Vector2>();
            var joyInput = _actionMap.Player.JoystickAim.ReadValue<Vector2>();

            var finalPos = mouseInput;

            // Normalize aim to be player relative if input is coming from a mouse.
            if (joyInput.magnitude > 0)
            {
                float y = ((RectTransform)transform).rect.height / 2;
                float x = ((RectTransform)transform).rect.width / 2;
                finalPos = new Vector2(x, y) + joyInput * joystickAimDistance;
            }

            CrosshairPos.position = finalPos;
        }
    }
}
