using UnityEngine;
using UnityEngine.EventSystems;

namespace LCPS.SlipForge.UI
{
    [RequireComponent(typeof(CanvasObjectSelector))]
    [RequireComponent(typeof(InventoryMenu))]
    public class TabSelector : MonoBehaviour
    {
        [SerializeField] private SelectEventRaiser[] _tabs;
        private CanvasObjectSelector _canvasObjectSelector;
        private InventoryMenu _inventoryMenu;
        private int _currentTab = 0;

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            foreach (var tab in _tabs)
            {
                tab.SelectEvent -= UpdateContent;
            }
            _inventoryMenu.OnMenuToggled -= SelectCurrentTab;
        }

        private void Initialize()
        {
            _canvasObjectSelector = GetComponent<CanvasObjectSelector>();
            _canvasObjectSelector.FirstSelectedObject = _tabs[0].gameObject;
            _inventoryMenu = GetComponent<InventoryMenu>();
            _inventoryMenu.OnMenuToggled += SelectCurrentTab;

            foreach (var tab in _tabs)
            {
                // run the UpdateContent method when the tab is selected
                tab.SelectEvent += UpdateContent;
            }
        }

        private void SelectCurrentTab(bool isOpen)
        {
            if (!isOpen) return;
            EventSystem.current.SetSelectedGameObject(_tabs[_currentTab].gameObject);
        }

        private void UpdateContent(SelectEventRaiser tab)
        {
            for (int i = 0; i < _tabs.Length; i++)
            {
                if (_tabs[i] == tab)
                {
                    _currentTab = i;
                    _canvasObjectSelector.FirstSelectedObject = _tabs[i].gameObject;
                    EnableContent(_tabs[i]);
                }
                else
                {
                    DisableContent(_tabs[i]);
                }
            }
        }

        private void EnableContent(SelectEventRaiser t)
        {
            // move the selected window to the front
            t.transform.parent.SetAsLastSibling();
            // set all children of the tab's parent to active
            foreach (Transform child in t.transform.parent)
            {
                child.gameObject.SetActive(true);
            }
        }

        private void DisableContent(SelectEventRaiser t)
        {
            foreach (Transform child in t.transform.parent)
            {
                if (child.gameObject != t.gameObject)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}
