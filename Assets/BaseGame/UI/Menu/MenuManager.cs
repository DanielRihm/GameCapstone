using UnityEngine;

namespace LCPS.SlipForge.UI
{
    public class MenuManager : MonoBehaviour
    {
        private ActionMap _inputScheme;

        // Start is called before the first frame update
        void Start()
        {
            // set up inputs
            _inputScheme = new ActionMap();
            _inputScheme.UI.Back.performed += ctx => OnBack();
            _inputScheme.Enable();
        }

        void OnDestroy()
        {
            _inputScheme.Disable();
        }

        private void OnBack()
        {
            if (DataTracker.Instance.IsMenuOpen())
            {
                DataTracker.Instance.MenuStack.Peek().Close(); // IsMenuOpen() ensures the stack is not empty
            }
        }
    }
}
