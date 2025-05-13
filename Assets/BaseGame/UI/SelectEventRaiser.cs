using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCPS.SlipForge.UI
{
    [RequireComponent(typeof(Selectable))]
    public class SelectEventRaiser : MonoBehaviour, ISelectHandler
    {
        public event Action<SelectEventRaiser> SelectEvent;

        public void OnSelect(BaseEventData eventData)
        {
            SelectEvent?.Invoke(this);
        }
    }
}
