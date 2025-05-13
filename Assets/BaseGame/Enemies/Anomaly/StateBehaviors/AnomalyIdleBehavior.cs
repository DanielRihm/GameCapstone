using System.Collections;
using System.Collections.Generic;
using LCPS.SlipForge.Enemy;
using LCPS.SlipForge.Enemy.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(NavMeshAgent))]
public class AnomalyIdleBehavior : MonoBehaviour, IEnemyState
{
    private Coroutine _routine;
    private NavMeshAgent _agent;
    private Enemy _self;

    [SerializeField] private List<Enemy> _allies;
    [SerializeField] private AudioClip _warpSound;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _self = GetComponent<Enemy>();

        // Sphere cast for nearby allies
        var hitColliders = Physics.OverlapSphere(transform.position, 10f, _self.EnemyMask);
        _allies = new List<Enemy>();
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null && enemy != _self)
            {
                _allies.Add(enemy);
            }
        }

        Assert.IsNotNull(_warpSound, "Anomaly warp sound is null.");
        SoundManager.Instance.RegisterSFX(_warpSound.name, _warpSound);

        Assert.IsNotNull(_agent, $"{this.name} requires a NavMeshAgent");
    }

    private void OnHit(float hp)
    {
        _self.State.Value = EnemyBrain.EnemyBehaviorState.Seek;

        // Triggering my all my allies
        foreach(var ally in _allies)
        {
            ally.State.Value = EnemyBrain.EnemyBehaviorState.Seek;
        }
    }

    public EnemyBrain.EnemyBehaviorState GetStateType()
    {
        return EnemyBrain.EnemyBehaviorState.Idle;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        _self.Health.OnValueChanged += OnHit;
        _routine = StartCoroutine(Teleport());
    }

    void OnDisable()
    {
        _self.Health.OnValueChanged -= OnHit;
        StopCoroutine(_routine);
    }
    
    private IEnumerator Teleport()
    {
        while(true)
        {
            // Wait an ammount of time before teleporting
            var waitTime = Random.Range(5, 20);

            // Jitter will occur only for the last 2 seconds of the wait time
            yield return new WaitForSeconds(waitTime - 2);
            waitTime = 2;

            // Sinosusal jitter to telegrapht the teleport wher the frequencey increases as time decreases
            var start = Time.time;
            var jitterPos = transform.position;
            while (Time.time < start + waitTime)
            {
                var normalizeTime = (Time.time - start) / waitTime;
                var frequendcy = 1 / (1 - normalizeTime);
                var jitter = Mathf.Sin(Time.time * frequendcy) * 0.05f;


                _agent.velocity = Vector3.zero;
                // Keep jitter small
                _agent.Warp(jitterPos + Vector3.right * jitter * 10);
                yield return null;
            }

            // Get a random point within the ageents navmesh
            var destination = GetRandomPointOnNavMesh();
            SoundManager.Instance.PlaySFX(_warpSound.name);
            _agent.Warp(destination);
            
        }
    }

    private Vector3 GetRandomPointOnNavMesh()
    {
        var randomRange = Random.Range(1, 10);
        var randomDirection = Random.insideUnitCircle.normalized;
        var direction = new Vector3(randomDirection.x, 0, randomDirection.y) * randomRange;

        if (NavMesh.SamplePosition(transform.position + direction, out var hit, 10, _agent.areaMask))
        {
            return hit.position;
        }

        return transform.position;
    }

}
