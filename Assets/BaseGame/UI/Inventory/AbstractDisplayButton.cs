using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCPS.SlipForge.UI
{
    [RequireComponent(typeof(Button))]
    public class AbstractDisplayButton : MonoBehaviour
    {
        public static event Action<AbstractDisplayButton> OnDisplayButtonClicked;

        [SerializeField] protected GameObject DisplayDesc;
        [SerializeField] protected GameObject HideDesc;
        protected Button _button;
        protected bool _isInitialized = false;

        protected virtual void OnEnable()
        {
            if (!_isInitialized) Initialize();
            _button.interactable = true;
        }

        protected virtual void OnDestroy()
        {
            OnDisplayButtonClicked -= OnDisplayClick;
        }

        protected virtual void Initialize()
        {
            _isInitialized = true;
            DisplayDesc.SetActive(false);
            _button = GetComponent<Button>();
            _button.onClick.AddListener(delegate { ViewEvent(); });
            _button.interactable = true;


            OnDisplayButtonClicked += OnDisplayClick;
        }

        protected virtual void OnDisplayClick(AbstractDisplayButton button)
        {
            if (button == this) return;
            _button.interactable = true; // re-enable the button if another button is clicked
        }

        protected virtual void ViewEvent(bool select = true)
        {
            OnDisplayButtonClicked?.Invoke(this);

            HideDesc.SetActive(false);
            DisplayDesc.SetActive(true);
            _button.interactable = false;
            if (select) EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}
