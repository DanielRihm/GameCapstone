using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LCPS.SlipForge
{
    public class SelectionTracker : MonoBehaviour
    {
        public static event Action OnSelectionChange;
        public static SelectionTracker Instance { get; private set; }
        private int? _selectedID = null;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            int? currentID = EventSystem.current.currentSelectedGameObject == null ? null : EventSystem.current.currentSelectedGameObject.GetInstanceID();
            if (_selectedID == currentID) return;

            _selectedID = currentID;
            OnSelectionChange?.Invoke();
        }

        public void InvokeSelectionChangeEvent()
        {
            OnSelectionChange?.Invoke();
        }
    }
}
