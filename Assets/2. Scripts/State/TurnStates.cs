using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnStateType
{
    StartTurn,
    MoveToTarget,
    Act,
    Return,
    EndTurn
}

public interface IUnitFsmControllable
{
    void ChangeUnitState(Enum newState);
    bool IsAtTargetPosition { get; }
    bool IsAnimationDone    { get; }
}

public class TurnStateMachine
{
    private ITurnState currentState;
    private Unit owner;

    public void Initialize(Unit unit, ITurnState entryState)
    {
        owner = unit;
        ChangeState(entryState);
    }

    public void Update()
    {
        currentState?.OnUpdate(owner);
    }

    public void ChangeState(ITurnState state)
    {
        currentState?.OnExit(owner);
        currentState = state;
        currentState.OnEnter(owner);
    }
}

public class StartTurnState : ITurnState
{
    public void OnEnter(Unit unit)
    {
        Debug.Log($"{unit.name} StartTurn State");
        if (unit.CurrentAction == ActionType.Attack || unit.CurrentAction == ActionType.Skill)
        {
            bool isMelee = unit.CurrentAttackAction != null &&
                           unit.CurrentAttackAction.DistanceType == AttackDistanceType.Melee;
            unit.ChangeTurnState(isMelee ? TurnStateType.MoveToTarget : TurnStateType.Act);
        }
    }

    public void OnUpdate(Unit unit)
    {
    }

    public void OnExit(Unit unit)
    {
    }
}

public class MoveToTargetState : ITurnState
{
    public void OnEnter(Unit unit)
    {
        unit.EnterMoveState();
    }

    public void OnUpdate(Unit unit)
    {
    }

    public void OnExit(Unit unit)
    {
    }
}

public class ActState : ITurnState
{
    private bool advanced;
    private Action onEnd;

    public void OnEnter(Unit unit)
    {
        advanced = false;
        onEnd = () =>
        {
            if (advanced)
            {
                return;
            }

            advanced = true;
            ProceedToNextState(unit);
        };

        if (unit.CurrentAction == ActionType.Attack)
        {
            unit.OnHitFinished += onEnd;
            unit.EnterAttackState();
        }
        else if (unit.CurrentAction == ActionType.Skill)
        {
            unit.OnSkillFinished += onEnd;
            unit.EnterSkillState();
        }
    }

    public void OnUpdate(Unit unit)
    {
    }

    public void OnExit(Unit unit)
    {
        unit.OnHitFinished -= onEnd;
        unit.OnSkillFinished -= onEnd;
        onEnd = null;
    }

    private void ProceedToNextState(Unit unit)
    {
        if (unit.IsDead)
        {
            unit.ChangeTurnState(TurnStateType.EndTurn);
            return;
        }

        bool isMelee = unit.CurrentAttackAction != null &&
                       unit.CurrentAttackAction.DistanceType == AttackDistanceType.Melee;
        unit.ChangeTurnState(isMelee ? TurnStateType.Return : TurnStateType.EndTurn);
    }
}

public class ReturnState : ITurnState
{
    public void OnEnter(Unit unit)
    {
        unit.EnterReturnState();
    }

    public void OnUpdate(Unit unit) { }
    public void OnExit(Unit unit)   { }
}

public class EndTurnState : ITurnState
{
    public void OnEnter(Unit unit)
    {
        Debug.Log("EndTurn State");
        unit.StartCoroutine(DelayEndTurn(unit));
    }

    public void OnUpdate(Unit unit) { }
    public void OnExit(Unit unit)   { }

    private IEnumerator DelayEndTurn(Unit unit)
    {
        yield return new WaitForFixedUpdate();
        unit.EndTurn();
    }
}