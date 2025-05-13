using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace LCPS.SlipForge
{
    [RequireComponent(typeof(Enemy.Enemy))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class BossSpeedUp : MonoBehaviour
    {
        public float SpeedupFactor = 2;

        private Enemy.Enemy _self;
        private NavMeshAgent _agent;

        private float _startHP;
        private float _startSpeed;

        // Start is called before the first frame update
        void Start()
        {
            _self = GetComponent<Enemy.Enemy>();
            _agent = GetComponent<NavMeshAgent>();

            _startHP = _self.Health.Value;
            _startSpeed = _agent.speed;
        }

        // Update is called once per frame
        void Update()
        {
            var maxSpeed = _startSpeed * SpeedupFactor;
            var percentHp = _self.Health.Value / _startHP;
            _agent.speed = Mathf.Lerp(maxSpeed, _startSpeed, percentHp);
        }
    }
}
