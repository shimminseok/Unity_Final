using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatManager))]
[RequireComponent(typeof(StatusEffectManager))]
[RequireComponent(typeof(Collider))]
public abstract class BaseController<TController, TState> : Unit, IAttackable, IDamageable where TController : BaseController<TController, TState>
    where TState : Enum

{
    [SerializeField] AttackTypeSO attackTypeSo;
    public StatusEffectManager StatusEffectManager { get; private set; }
    public BaseEmotion         CurrentEmotion      { get; private set; }
    private StateMachine<TController, TState> stateMachine;

    private IState<TController, TState>[] states;

    public          TState      CurrentState { get; protected set; }
    public          int         Index        { get; protected set; }
    public          bool        IsDead       { get; protected set; }
    public abstract StatBase    AttackStat   { get; protected set; }
    public abstract IDamageable Target       { get; protected set; }

    protected AttackTypeSO AttackTypeSo => attackTypeSo;

    protected virtual void Awake()
    {
        StatManager = GetComponent<StatManager>();
        StatusEffectManager = GetComponent<StatusEffectManager>();
        stateMachine = new StateMachine<TController, TState>();
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

    public void ChangeEmotion(Emotion emotion)
    {
        if (CurrentEmotion.EmotionType != emotion)
        {
            CurrentEmotion = EmotionFactory.CreateEmotion(emotion);
        }
        else
        {
            CurrentEmotion.Stack++;
            //어쩌구 저쩌구 스택을 올려준다~
        }
    }

    protected abstract IState<TController, TState> GetState(TState state);

    public abstract void Attack();
    public abstract void TakeDamage(float amount, StatModifierType modifierType = StatModifierType.Base);

    public abstract void Dead();
}