using UnityEngine;
using UnityEngine.EventSystems;

namespace LCPS.SlipForge.UI
{
    public class SelectionCursor : MonoBehaviour
    {
        private void Start()
        {
            SelectionTracker.OnSelectionChange += UpdateCursor;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            SelectionTracker.OnSelectionChange -= UpdateCursor;
        }

        private void UpdateCursor()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                // if there is no selected object, hide the cursor
                gameObject.SetActive(false);
                return;
            }

            // if there is a selected object, show the cursor
            gameObject.SetActive(true);
            // place the cursor at the left edge of the selected object
            RectTransform rect = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>();
            //Vector3 pos = rect.position;
            // pos.x -= rect.rect.width / 2 + GetComponent<RectTransform>().rect.width / 2;
            // need to subtract the global width of the object and cursor so resolution/scale doesn't affect the position
            float xPos = rect.GetGlobalEdges().left - CursorWidth() / 2;
            transform.position = new Vector3(xPos, rect.position.y, rect.position.z);
        }

        private float CursorWidth()
        {
            return GetComponent<RectTransform>().GetGlobalSize().width;
        }
    }
}
