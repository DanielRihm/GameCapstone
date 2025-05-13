using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCPS.SlipForge.Enemy.AI
{
    public class TimedBehavior : BehaviorBase
    {
        public EnemyBrain.EnemyBehaviorState NextState;
        public int Seconds;

        private void OnEnable()
        {
            StartCoroutine(ChangeState());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public IEnumerator ChangeState()
        {
            yield return new WaitForSeconds(Seconds);

            Brain.State.Value = NextState;
        }
    }
}

