using UnityEngine;

namespace LCPS.SlipForge
{
    /// <summary>
    /// Rotate the object transform with constant speed
    /// </summary>
    public class RotateTransform : MonoBehaviour
    {
        [SerializeField]
        private float RotationSpeed;

        [SerializeField]
        private Vector3 RotationAxis;// = Vector3.up;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // Rotate the object
            transform.Rotate(RotationSpeed * Time.deltaTime * RotationAxis);
        }
    }
}
