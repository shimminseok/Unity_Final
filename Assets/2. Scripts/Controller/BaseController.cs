using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatManager))]
[RequireComponent(typeof(StatusEffectManager))]
[RequireComponent(typeof(Collider))]
public abstract class BaseController<TController, TState> : Unit where TController : BaseController<TController, TState>
    where TState : Enum

{
    private StateMachine<TController, TState> stateMachine;

    private IState<TController, TState>[] states;

    public TState CurrentState { get; protected set; }

    protected virtual void Awake()
    {
        StatManager = GetComponent<StatManager>();
        StatusEffectManager = GetComponent<StatusEffectManager>();
        stateMachine = new StateMachine<TController, TState>();
        Collider = GetComponent<CapsuleCollider>();
        CreateEmotion();
        SetupState();
    }

    protected virtual void Start()
    {
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
}