using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.UI
{
    public class InteractionHandler : MonoBehaviour
    {
        private ActionMap _inputScheme;

        // Start is called before the first frame update
        void Start()
        {
            TriggerActionInvoker.OnEnter += OnInteractionEnter;
            TriggerActionInvoker.OnExit += OnInteractionExit;

            _inputScheme = new ActionMap();
            _inputScheme.Player.Interact.performed += ctx => OnInteract();
            _inputScheme.Enable();
        }

        private void OnInteractionEnter(TriggerActionInvoker invoker, Collider other, string tag)
        {
            Assert.IsNull(DataTracker.Instance.ActiveTrigger, "Multiple active triggers");
            DataTracker.Instance.ActiveTrigger = invoker;
        }

        private void OnInteractionExit(Collider other)
        {
            Assert.IsNotNull(DataTracker.Instance.ActiveTrigger, "No active trigger but exit was registered");
            DataTracker.Instance.ActiveTrigger = null;
        }

        private void OnInteract() => DataTracker.Instance.InteractionWithActiveTrigger();

        private void OnDestroy()
        {
            TriggerActionInvoker.OnEnter -= OnInteractionEnter;
            TriggerActionInvoker.OnExit -= OnInteractionExit;
            _inputScheme.Disable();
        }
    }
}
