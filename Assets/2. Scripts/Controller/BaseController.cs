using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatManager))]
[RequireComponent(typeof(StatusEffectManager))]
[RequireComponent(typeof(Collider))]
public abstract class BaseController<TController, TState> : MonoBehaviour, IAttackable, IDamageable where TController : BaseController<TController, TState>
    where TState : Enum

{
    public StatManager         StatManager         { get; private set; }
    public StatusEffectManager StatusEffectManager { get; private set; }

    private StateMachine<TController, TState> stateMachine;
    private IState<TController, TState>[] states;
    public          TState      CurrentState { get; protected set; }
    public          bool        IsDead       { get; protected set; }
    public          Collider    Collider     { get; protected set; }
    public abstract StatBase    AttackStat   { get; protected set; }
    public abstract IDamageable Target       { get; protected set; }

    protected virtual void Awake()
    {
        StatManager = GetComponent<StatManager>();
        StatusEffectManager = GetComponent<StatusEffectManager>();
        stateMachine = new StateMachine<TController, TState>();
        Collider = GetComponent<CapsuleCollider>();
    }

    protected virtual void Start()
    {
        SetupState();
        AttackStat = StatManager.GetStat<ResourceStat>(StatType.AttackPow);
    }

    protected virtual void Update()
    {
        TryStateTransition();
        stateMachine?.Update();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine?.FixedUpdate();
    }

    private void SetupState()
    {
        Array values = Enum.GetValues(typeof(TState));
        states = new IState<TController, TState>[values.Length];
        for (int i = 0; i < states.Length; i++)
        {
            TState state = (TState)values.GetValue(i);
            states[i] = GetState(state);
        }

        CurrentState = (TState)values.GetValue(0);
        stateMachine.Setup((TController)this, states[0]);
    }


    protected void ChangeState(TState newState)
    {
        stateMachine.ChangeState(states[Convert.ToInt32(newState)]);
        CurrentState = newState;
    }

    private void TryStateTransition()
    {
        int currentIndex = Convert.ToInt32(CurrentState);
        var next         = states[currentIndex].CheckTransition((TController)this);
        if (!next.Equals(CurrentState))
        {
            ChangeState(next);
        }
    }

    protected abstract IState<TController, TState> GetState(TState state);

    public abstract void Attack();
    public abstract void TakeDamage(IAttackable attacker);

    public abstract void Dead();
}