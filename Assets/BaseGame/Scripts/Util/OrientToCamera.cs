using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCPS.SlipForge
{
    public class OrientToCamera : MonoBehaviour
    {
        private Camera _mainCamera;
        // Start is called before the first frame update
        void Start()
        {
            _mainCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            if (_mainCamera != null)
            {
                transform.forward = _mainCamera.transform.forward;
                transform.up = _mainCamera.transform.up;
            }
        }
    }
}
