using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCPS.SlipForge.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class DropdownScroller : MonoBehaviour
    {
        private ScrollRect _scrollRect;
        private RectTransform _contentPanel;

        private void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _contentPanel = _scrollRect.content;
        }

        private void OnEnable()
        {
            StartCoroutine(EnableNextFrame());
        }

        private void OnDisable()
        {
            SelectionTracker.OnSelectionChange -= ScrollToSelection;
        }

        private IEnumerator EnableNextFrame()
        {
            yield return null;
            // GetComponent<Canvas>().sortingOrder = 90; // this ensures that the dropdown remains behind the cursor
            SelectionTracker.OnSelectionChange += ScrollToSelection;
            ScrollToSelection();
        }

        private void ScrollToSelection()
        {
            if (EventSystem.current.currentSelectedGameObject == null) return;
            if (EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>() == null) return;

            RectTransform targetTransform = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>();
            Debug.Log("Scrolling to: " + EventSystem.current.currentSelectedGameObject.name);

            // get position of target and offset (i.e. targetPosition.y + offset = top of target)
            // targetY is the relative y position of the target within the scroll view
            float offset = GetHalfTransformHeight(targetTransform);
            float targetY = GetTransformScrollPosition(targetTransform);

            // if the target is within the scroll view, do not scroll
            (float top, float bottom) = GetVisibleBounds();
            if (targetY < top && targetY > bottom) return;

            // if the target is above the scroll view, scroll up
            // else if the target is below the scroll view, scroll down
            if (targetY > top) ScrollToY(targetY + top + offset);
            else if (targetY < bottom) ScrollToY(targetY - bottom - offset);

            // since scrolling has moved the target, retrigger the selection event
            // this shouldn't cause an infinite loop because the target is now within the scroll view
            SelectionTracker.Instance.InvokeSelectionChangeEvent();
        }

        private float GetTransformScrollPosition(RectTransform transform)
        {
            return _scrollRect.viewport.InverseTransformPoint(transform.position).y;
        }

        private float GetHalfTransformHeight(RectTransform transform)
        {
            return transform.rect.height / 2;
        }

        private void ScrollToY(float targetY)
        {
            Canvas.ForceUpdateCanvases();
            float newY = _scrollRect.transform.InverseTransformPoint(_contentPanel.position).y - targetY;
            _contentPanel.anchoredPosition = new Vector2(_contentPanel.anchoredPosition.x, newY);
        }

        private (float top, float bottom) GetVisibleBounds()
        {
            // get min and max y values
            (float _, float _, float top, float bottom) = _scrollRect.viewport.GetGlobalEdges();
            return (_scrollRect.viewport.InverseTransformPoint(new Vector3(0, top, 0)).y, _scrollRect.viewport.InverseTransformPoint(new Vector3(0, bottom, 0)).y);
        }
    }
}
