using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Enemy.AI
{
    public class EnemyInterruptBehavior : MonoBehaviour, IEnemyState
    {
        public float InterruptDuration = 0.25f;

        private Enemy _self;

        // Start is called before the first frame update
        void OnEnable()
        {
            _self = GetComponent<Enemy>();

            Assert.IsNotNull(_self, $"{this.name} requires an Enemy component.");

            StartCoroutine(Interrupt());
        }
        
        private IEnumerator Interrupt()
        {
            yield return new WaitForSeconds(InterruptDuration);
            _self.Interrupted.Value = false;
        }

        // Update is called once per frame
        void OnDisable()
        {
            StopAllCoroutines();
        }

        public EnemyBrain.EnemyBehaviorState GetStateType()
        {
            return EnemyBrain.EnemyBehaviorState.Interrupt;
        }
    }
}
