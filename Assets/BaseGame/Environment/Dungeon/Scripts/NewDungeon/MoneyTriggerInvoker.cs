

using LCPS.SlipForge.Engine;
using UnityEngine;

namespace LCPS.SlipForge
{
    public class MoneyTriggerInvoker : TriggerActionInvoker
    {
        private Observable<int> _chipObserver;
        public int DropAmount = 200;

        public override void Interact()
        {
            Debug.Log("HIT");
            _chipObserver = DataTracker.GetObservable(data => data.SaveData.Currency.PotatoChips);
            _chipObserver.Value += DropAmount;


            Destroy(this.gameObject);
        }

    }
}
