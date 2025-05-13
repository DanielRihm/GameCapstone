using LCPS.SlipForge.UI;

namespace LCPS.SlipForge
{
    public static class DataTrackerMenuUtilities
    {
        public static void CloseAllMenus(this DataTracker data)
        {
            // note that this does not guarantee that all menus are successfully closed
            foreach (var menu in data.MenuStack)
            {
                menu.Close();
            }
        }

        public static bool IsTopMenu(this DataTracker data, IMenu menu)
        {
            // if the stack is empty, return false
            // else return true if the top menu is the given menu
            // IsMenuOpen() ensures the top menu is open and valid
            return data.IsMenuOpen() && data.MenuStack.Peek() == menu;
        }

        public static bool IsMenuOpen(this DataTracker data)
        {
            // no menu is open if the stack is empty
            if (data.MenuStack.Count == 0) return false;

            // if the top menu is not open, pop it off the stack
            if (!IsThisMenuOpen(data.MenuStack.Peek()))
            {
                data.MenuStack.Pop();
                return data.IsMenuOpen(); // check the next menu
            }

            // if the top menu is open, return true
            return true;
        }

        private static bool IsThisMenuOpen(IMenu menu)
        {
            //Debug.Log("IsMenuValid: " + (ActiveMenu?.IsValid ?? false) +
            //    "\nIsMenuOpen: " + ((ActiveMenu?.IsValid ?? false) && ActiveMenu.IsOpen()) +
            //    "\nMenuType: " + (ActiveMenu is Pause ? "Pause" : "NotPause"));

            // if ActiveMenu is not null, and is valid, and is open, return true
            return (menu?.IsValid ?? false) && menu.IsOpen();
        }
    }
}
