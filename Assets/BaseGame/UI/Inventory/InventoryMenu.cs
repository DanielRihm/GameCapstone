using System;
using System.Collections;
using UnityEngine;

namespace LCPS.SlipForge.UI
{
    public class InventoryMenu : Menu
    {
        // the boolean is true if the menu is open, false if it is closed AFTER the toggle is completed
        public event Action<bool> OnMenuToggled;

        private ActionMap _inputScheme;

        protected override void Start()
        {
            base.Start();

            _inputScheme = new ActionMap();
            _inputScheme.Player.Inventory.performed += ctx => StartCoroutine(InventoryActionPerformed());
            _inputScheme.Enable();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _inputScheme.Disable();
        }

        private IEnumerator InventoryActionPerformed()
        {
            ToggleMenu();
            yield return null;
            yield return new WaitForEndOfFrame(); // prevent race condition between base.CloseNextFrame() and IsOpen()
            bool isOpen = IsOpen();
            OnMenuToggled?.Invoke(isOpen);
        }
    }
}
