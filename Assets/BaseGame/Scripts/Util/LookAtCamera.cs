using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCPS.SlipForge
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera _mainCamera;
        // Start is called before the first frame update
        void OnEnable()
        {
            _mainCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            if (_mainCamera != null)
            {
                transform.up = _mainCamera.transform.up;
                transform.forward = _mainCamera.transform.forward;
            }
        }
    }
}
