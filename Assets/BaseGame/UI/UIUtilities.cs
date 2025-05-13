using System.Collections;
using System.Linq;
using UnityEngine;

namespace LCPS.SlipForge.UI
{
    public static class UIUtilities
    {
        public static (float width, float height) GetLocalSize(this RectTransform rectTransform)
        {
            Vector2 size = rectTransform.rect.size;
            return (size.x, size.y);
        }

        public static (float width, float height) GetGlobalSize(this RectTransform rectTransform)
        {
            float width = rectTransform.rect.width * rectTransform.lossyScale.x;
            float height = rectTransform.rect.height * rectTransform.lossyScale.y;
            return (width, height);

            //(float left, float right, float top, float bottom) edges = rectTransform.GetGlobalEdges();
            //float width = edges.right - edges.left;
            //float height = edges.top - edges.bottom;
            //return (width, height);
        }

        public static (float left, float right, float top, float bottom) GetGlobalEdges(this RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            float left = corners.Select(x => x.x).Min();
            float right = corners.Select(x => x.x).Max();
            float top = corners.Select(x => x.y).Max();
            float bottom = corners.Select(x => x.y).Min();
            return (left, right, top, bottom);
        }
    }
}
