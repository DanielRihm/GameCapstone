using System;
using System.Collections;
using System.Collections.Generic;
using LCPS.SlipForge.Enemy;
using LCPS.SlipForge.Enemy.AI;
using LCPS.SlipForge.Engine;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Animator))]
public class EnemyBrain : MonoBehaviour
{
    [Tooltip("HP threashold at which the enemy will flee. Set it very negative to prevent freeing.")]
    [SerializeField] public float LowHPThreashold = -100;
    [SerializeField] private Observable<EnemyBehaviorState> State;

    private Dictionary<EnemyBehaviorState, MonoBehaviour> _behaviors = new();
    private Observable<bool> Interrupted => _enemy.Interrupted;
    private EnemyBehaviorState _memmory;

    private Enemy _enemy;
    private Animator _animator;

    // Start is called before the first frame update
    void OnEnable()
    {
        _enemy = GetComponent<Enemy>();
        _animator = GetComponent<Animator>();

        // Proxy to the enemy state
        State = _enemy.State;
        State.OnValueChanged += EnemyStateChange;
        _enemy.Health.OnValueChanged += OnHalthChange;
        Interrupted.OnValueChanged += OnInterrupted;

        // Register any state behavior scripts
        var states = GetComponents<IEnemyState>();
        foreach(var state in states)
        {
            if(state is MonoBehaviour behavior)
            {
                Assert.IsFalse(behavior.enabled, $"Enemy State Behavior \"{behavior.GetType()}\" should start disabled.");

                var stateType = state.GetStateType();
                if(!_behaviors.ContainsKey(stateType))
                {
                    _behaviors.Add(stateType, behavior);
                }
            }
        }

        StartCoroutine(OffsetAnimator());
    }

    private IEnumerator OffsetAnimator()
    {
        _animator.enabled = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.0f, 0.5f));
        _animator.enabled = true;
    }

    private void OnHalthChange(float hp)
    {
        if(hp < LowHPThreashold) {
            State.Value = EnemyBehaviorState.Flee;
        }
    }

    private void OnInterrupted(bool interrupted)
    {
        if(interrupted)
        {
            _memmory = State.Value;
            EnemyStateChange(EnemyBehaviorState.Interrupt);
        }
        else
        {
            EnemyStateChange(_memmory);
        }
    }

    private void EnemyStateChange(EnemyBehaviorState newState)
    {
        // Trigger the animator
        if(_animator != null)
        {
            _animator.SetTrigger(newState.ToString());
        }
    }

    public MonoBehaviour GetStateBehavior(EnemyBehaviorState state)
    {
        if (_behaviors.ContainsKey(state))
        {
            return _behaviors[state];
        }

        return null;
    }

    public void SetState(EnemyBehaviorState state)
    {
        State.Value = state;
    }

    public void DoAttack()
    {
        State.Value = EnemyBehaviorState.Attack;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum EnemyBehaviorState
    {
        Idle,
        Seek,
        Flee,
        Attack,
        Telegraph,
        Interrupt,
        Die
    }
}
