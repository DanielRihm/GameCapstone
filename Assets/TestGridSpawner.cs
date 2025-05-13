using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge
{
    public class TestGridSpawner : MonoBehaviour
    {
        [SerializeField] private AreaEnemySpawner _spawner;

        private BoxCollider _collider;
        // Start is called before the first frame update
        void Start()
        {
            _collider = GetComponent<BoxCollider>();
            Assert.IsNotNull(_collider, "Collider is null.");
            Assert.IsNotNull(_spawner, "Spawner is null.");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerInstance>() is PlayerInstance player)
            {
                var enemies = GetEnemiesInGrid();
                if(enemies.Length == 0)
                {
                    _spawner.SpawnEnemies();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerInstance>() is PlayerInstance player)
            {
                // Remove all enemies
                foreach (var enemy in GetEnemiesInGrid())
                {
                    Destroy(enemy.gameObject);
                }
            }
        }

        private Enemy.Enemy[] GetEnemiesInGrid()
        {
            // search the children for enemies
            return GetComponentsInChildren<Enemy.Enemy>();
        }
    }
}
