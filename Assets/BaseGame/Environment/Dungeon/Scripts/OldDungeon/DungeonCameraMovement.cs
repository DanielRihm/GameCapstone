using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCPS.SlipForge
{
    public class CameraMovement : MonoBehaviour
    {
        public float moveSpeed = 5f;    // Speed for moving the camera
        private Quaternion initialRotation;  // To store the initial rotation of the camera

        void Start()
        {
            // Store the initial rotation of the camera when the game starts
            initialRotation = transform.rotation;
        }

        void Update()
        {
            // Handle movement on the x, y, and z planes (forward/backward, left/right, up/down)
            float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;  // A/D or Left/Right Arrow keys for left-right movement
            float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;    // W/S or Up/Down Arrow keys for forward-backward movement

            // Ascend/descend using Space (up) and Left Ctrl (down)
            float moveY = 0f;
            if (Input.GetKey(KeyCode.Space))
            {
                moveY = moveSpeed * Time.deltaTime;  // Move up
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                moveY = -moveSpeed * Time.deltaTime;  // Move down
            }

            // Apply the movement to the camera
            transform.Translate(new Vector3(moveX, moveY, moveZ));
        }
    }
}

