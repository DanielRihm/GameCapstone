using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Enemy.AI
{
    public class BehaviorSeekTrigger : BehaviorBase
    {
        public float Range = 10f;

        // Update is called once per frame
        void Update()
        {
            if(PlayerInstance.Instance == null)
            {
                return;
            }

            if (Vector3.Distance(PlayerInstance.Instance.transform.position, transform.position) < Range)
            {
                Brain.State.Value = EnemyBrain.EnemyBehaviorState.Seek;
            }
        }
    }
}
