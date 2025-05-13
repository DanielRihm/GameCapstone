using LCPS.SlipForge.Enemy.AI;
using LCPS.SlipForge.Engine;
using LCPS.SlipForge.Enum.Enemy;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;


namespace LCPS.SlipForge.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour, IEnemy
    {
        public Observable<EnemyBrain.EnemyBehaviorState> State = new();
        public Observable<float> Health = new(100);
        public Observable<bool> Interrupted = new(false);

        [SerializeField]
		public Transform Target;        
        [SerializeField]
        private EnemyType EnemyType;

		private NavMeshAgent _navAgent;
        public LayerMask EnemyMask;
        public float MaxHP;

		private void Awake()
		{
			_navAgent = GetComponent<NavMeshAgent>();

			Assert.IsNotNull(_navAgent);
			if (DataTracker.Instance.IsDungeon.Value)
			{
				var _parent = this.GetComponentInParent<DungeonFloor>();
				Assert.IsNotNull(_parent, "Parent of enemy is not a dungeon floor");
				DungeonManager.Instance.RegisterEnemy(_parent.Location, this.gameObject);
			}
		}

        private void Start()
        {
            MaxHP = Health.Value;
        }

		public EnemyType GetEnemyType()
		{
			return EnemyType;
		}

		bool isDead = false;
		public void TakeDamage(float amount)
		{
			Health.Value -= amount;
			if (Health.Value <= 0 && !isDead)
			{
                isDead = true;
                State.Value = EnemyBrain.EnemyBehaviorState.Die;
			}
		}

        public void AssignMovementSet(EnemyMovementSet set)
        {
            if(GetComponent<MovementSetSeekBehavior>() is MovementSetSeekBehavior behavior)
            {
                behavior.MovementSet = set;
            }
		}
	}

}


