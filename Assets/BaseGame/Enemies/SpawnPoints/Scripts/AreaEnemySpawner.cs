using LCPS.SlipForge.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge
{
    [SelectionBase]
    [RequireComponent(typeof(BoxCollider))]
    public class AreaEnemySpawner : MonoBehaviour, IEnemySpawner
    {
        [SerializeField]
        public EnemySpawnerData SpawnerData;

        [SerializeField]
        [Tooltip("The rate at which enemies spawn in enemies per second.")]
        public float SpawnRate = 1.0f;

        [SerializeField]
        [Tooltip("Should enemies spawn on start")]
        public bool SpawnOnStart = true;

        private BoxCollider _spawnVolume;

        private int _spawnIndex = 0;
        private Coroutine _spawnRoutine;

        private void OnEnable()
        {
            _spawnVolume = GetComponent<BoxCollider>();

            Assert.IsNotNull(SpawnerData, $"SpawnerData is missing on {gameObject.name}");
            Assert.IsNotNull(_spawnVolume, $"Collider is missing on {gameObject.name}");
        }

        void Awake()
        {
            if (DataTracker.Instance.IsDungeon.Value)
            {
                var _parent = this.GetComponentInParent<DungeonFloor>();
                Assert.IsNotNull(_parent, "Parent of enemy is not a dungeon floor");
                DungeonManager.Instance.RegisterSpawner(_parent.Location);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (SpawnOnStart)
            {
                SpawnEnemies();
            }
        }

        public void SpawnEnemies()
        {
            if (_spawnRoutine != null)
            {
                StopCoroutine(_spawnRoutine);
            }
            
            _spawnRoutine = StartCoroutine(SpawnEnemiesRoutine());
        }

        private IEnumerator SpawnEnemiesRoutine()
        {
            _spawnIndex = 0; // Support respawn
            
            while(_spawnIndex < SpawnerData.Pool.Count)
            {
                var spawnRule = SpawnerData.Pool[_spawnIndex];

                // Get a random position within the spawn volume
                var randomPosition = new Vector3(
                    Random.Range(-_spawnVolume.size.x / 2, _spawnVolume.size.x / 2),
                    0,
                    Random.Range(-_spawnVolume.size.z / 2, _spawnVolume.size.z / 2)
                );

                var position = transform.position + _spawnVolume.center + randomPosition;
                // The enemy and the spawner should be in the same parent transform
                var enemyInstance = Instantiate(spawnRule.EnemyDefinition.Prefab, transform.position + randomPosition, Quaternion.identity, transform.parent);
                enemyInstance.AssignMovementSet(spawnRule.Moves);
                
                _spawnIndex++;

                yield return null/*new WaitForSeconds(SpawnRate)*/;
            }
        }
    }
}
