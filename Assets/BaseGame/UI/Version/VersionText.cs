using TMPro;
using UnityEngine;

namespace LCPS.SlipForge
{
    [RequireComponent(typeof(TMP_Text))]
    public class VersionText : MonoBehaviour
    {
        private TMP_Text _text;

        void Start()
        {
            // Get the text component
            _text = GetComponent<TMP_Text>();

            // Update the version text
            _text.text = $"{Application.version}-{Application.unityVersion}";
        }
    }
}
