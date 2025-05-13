using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.UI
{
    public class MenuTriggerInvoker : TriggerActionInvoker
    {
        [SerializeField] private GameObject MenuObject;
        private IMenu menu;

        private void Start()
        {
            menu = MenuObject.GetComponent<IMenu>();
            Assert.IsNotNull(menu, "Menu is null");
        }

        public override void Interact()
        {
            Assert.IsNotNull(menu, "Menu is null");
            menu?.Open(); // this handles the rest of the menu logic
        }
    }
}
