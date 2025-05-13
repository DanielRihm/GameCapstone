using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LCPS.SlipForge.UI
{
    public class InventoryButton : AbstractDisplayButton
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            StartCoroutine(SelectInventoryNextFrame());
        }

        private IEnumerator SelectInventoryNextFrame()
        {
            yield return null;
            ViewEvent(false);
        }
    }
}
