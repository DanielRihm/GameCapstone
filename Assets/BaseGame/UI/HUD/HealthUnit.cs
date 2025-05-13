using UnityEngine;

namespace LCPS.SlipForge.UI
{
    public class HealthUnit : MonoBehaviour
    {
        [SerializeField] private GameObject Filler;
        public bool IsFilled
        {
            get => Filler.activeSelf;
            set => Filler.SetActive(value);
        }
    }
}
