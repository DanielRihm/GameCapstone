using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LCPS.SlipForge.UI
{
    public class Menu : MonoBehaviour, IMenu
    {
        [SerializeField] protected GameObject MenuCanvas;
        private bool _isValid = false;

        public bool IsValid { get { return _isValid; } }

        private float previousTimescale;

        protected virtual void Start()
        {
            _isValid = true;
            MenuCanvas.SetActive(false);
        }

        protected virtual void OnDestroy()
        {
            //Close(); // can't close here because next frame the menu will be destroyed
            _isValid = false;

            // if the menu is destroyed then the game is no longer paused
            // this could cause issues if other game functions mess with the timescale
            Time.timeScale = 1;
        }

        public virtual void Open(bool doStacking = false)
        {
            // If no other menu is open, open this menu
            if (doStacking || !DataTracker.Instance.IsMenuOpen())
            {
                DataTracker.Instance.MenuStack.Push(this);
                MenuCanvas.SetActive(true);
                MenuCanvas.GetComponent<Canvas>().sortingOrder = DataTracker.Instance.MenuStack.Count;
                EventSystem.current.SetSelectedGameObject(null); // this ensures that items on the previous menu are not selected
                previousTimescale = Time.timeScale;
                Time.timeScale = 0;
            }
        }

        public virtual void Close()
        {
            // this has to be done next frame to prevent input overlap issues
            StartCoroutine(CloseNextFrame());
        }

        protected virtual IEnumerator CloseNextFrame()
        {
            yield return null;
            MenuCanvas.SetActive(false);
            // generally good to clear the selected object when closing a menu
            EventSystem.current.SetSelectedGameObject(null);
            if (!DataTracker.Instance.IsMenuOpen()) Time.timeScale = 1;
            Time.timeScale = previousTimescale;
        }

        public virtual bool IsOpen()
        {
            return MenuCanvas.activeSelf;
        }

        protected virtual void ToggleMenu()
        {
            if (MenuCanvas.activeSelf)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
    }
}
