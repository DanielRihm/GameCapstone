using LCPS.SlipForge.Enemy.AI;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Enemy
{
    public abstract class BehaviorBase : MonoBehaviour, IEnemyState
    {
        public EnemyBrain.EnemyBehaviorState ActiveState;

        protected Enemy Brain;

        private void Awake()
        {
            Brain = GetComponent<Enemy>();
            Assert.IsNotNull(Brain, $"{this.name} requires an Enemy component");
        }

        public EnemyBrain.EnemyBehaviorState GetStateType()
        {
            return ActiveState;
        }
    }
}
