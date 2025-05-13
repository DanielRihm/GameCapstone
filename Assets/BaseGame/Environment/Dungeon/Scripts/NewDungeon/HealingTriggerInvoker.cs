

using UnityEngine;

namespace LCPS.SlipForge
{
    public class HealingTriggerInvoker : TriggerActionInvoker
    {
        public override void Interact()
        {
            Debug.Log("HIT");
            PlayerInstance.Instance.Heal(2);
            Destroy(this.gameObject);
        }

    }
}
