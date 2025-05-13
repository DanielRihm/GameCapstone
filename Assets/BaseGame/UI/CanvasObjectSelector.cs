using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace LCPS.SlipForge.UI
{
    public class CanvasObjectSelector : MonoBehaviour
    {
        public GameObject FirstSelectedObject;
        [SerializeField] private GameObject MenuObject;
        private IMenu firstObjectMenu;

        private void Start()
        {
            // this seems kind of hacky, but it works
            EventSystem.current.gameObject.GetComponent<InputSystemUIInputModule>().move.action.performed += OnMove;

            firstObjectMenu = MenuObject.GetComponent<IMenu>();
        }

        private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            // this case is covered by checking the top menu in the next block
            // if the first selected object is null or inactive, return
            // if (!IsFirstSelectedObjectActive()) return;

            // if the top menu is not the menu this object is associated with, return
            if (!DataTracker.Instance.IsTopMenu(firstObjectMenu)) return;

            // if no object is selected, select the first object
            // this is done next frame to prevent the event system from navigating in the same frame as selection
            StartCoroutine(SelectNextFrameIfDeselected());
        }

        private IEnumerator SelectNextFrameIfDeselected()
        {
            yield return null;
            if (EventSystem.current.currentSelectedGameObject == null && IsFirstSelectedObjectActive())
            {
                Debug.Log("Selecting first UI object");
                EventSystem.current.SetSelectedGameObject(FirstSelectedObject);
            }
        }

        private bool IsFirstSelectedObjectActive()
        {
            return FirstSelectedObject != null && FirstSelectedObject.activeSelf;
        }

        private void OnDestroy()
        {
            if (EventSystem.current != null)
                EventSystem.current.gameObject.GetComponent<InputSystemUIInputModule>().move.action.performed -= OnMove;
        }
    }
}
