using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.UI
{
    public class TitleMenu : Menu
    {

        // Use this for initialization
        protected override void Start()
        {
            Assert.IsFalse(DataTracker.Instance.IsMenuOpen(), "TitleMenu is being opened when another menu is already open");

            base.Start();
            Open(true);
            Time.timeScale = 1f; // the title screen should not be paused
        }

        public override void Close() { } // the title cannot be closed
    }
}
