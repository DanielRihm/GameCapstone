using LCPS.SlipForge.UI;
using System;
using UnityEngine;

namespace LCPS.SlipForge
{
    public abstract class TriggerActionInvoker : MonoBehaviour
    {
        [SerializeField] protected string Tag;
        [SerializeField] private string PlayerObjectName = "Player";
        [SerializeField] private GameObject InteractionPrefab; // this is a prefab that will be displayed when the player is near the object
        private GameObject _interactionObject;

        // static events so that these can be subscribed to by classes outside of this one's scene
        public static event Action<TriggerActionInvoker, Collider, string> OnEnter;
        public static event Action<Collider> OnExit;

        protected virtual void Awake()
        {
            // instantiate the prefab as a child of this object
            _interactionObject = Instantiate(InteractionPrefab, transform);

            // set the textmeshpro text to the tag of the object
            _interactionObject.GetComponentInChildren<InteractionTextHandler>().InteractionTag = Tag;

            // disable the object by default
            _interactionObject.SetActive(false);
        }

        // big brain move: use the instance variable to pass instance-specific data to the static events
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name != PlayerObjectName) return;
            OnEnter?.Invoke(this, other, Tag);
            _interactionObject.SetActive(true);
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name != PlayerObjectName) return;
            OnExit?.Invoke(other);
            _interactionObject.SetActive(false);
        }

        // extra big brain move: call the interaction action when the player interacts with the object using the instance variable passed from the static event
        public abstract void Interact(); // => InteractionAction.Invoke();
    }
}
