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
        switch (unit.CurrentAction)
        {
            case ActionType.Attack:
                if (unit.CurrentAttackAction.DistanceType == AttackDistanceType.Melee)
                {
                    unit.ChangeTurnState(TurnStateType.MoveToTarget);
                }
                else
                {
                    unit.ChangeTurnState(TurnStateType.Act);
                }

                break;
            case ActionType.SKill:
                if (unit.CurrentAttackAction.DistanceType == AttackDistanceType.Melee)
                {
                    unit.ChangeTurnState(TurnStateType.MoveToTarget);
                }
                else
                {
                    unit.ChangeTurnState(TurnStateType.Act);
                }

                break;
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
        Debug.Log($"{unit.name} MoveToTarget State");
        if (unit is PlayerUnitController)
        {
            unit.ChangeUnitState(PlayerUnitState.Move);
        }
        else if (unit is EnemyUnitController)
        {
            unit.ChangeUnitState(EnemyUnitState.Move);
        }
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
    private ICombatAction action;
    private Action handler;

    private Unit target;
    private Action onReactionEndHandler;

    public void OnEnter(Unit unit)
    {
        target = unit.Target as Unit;
        if (target != null)
        {
            target.SetLastAttacker(unit);
        }


        onReactionEndHandler = () =>
        {
            ProceedToNextState(unit);
        };
        unit.OnHitFinished += onReactionEndHandler;

        if (unit.CurrentAction == ActionType.Attack)
        {
            if (unit is PlayerUnitController)
            {
                unit.ChangeUnitState(PlayerUnitState.Attack);
            }
            else if (unit is EnemyUnitController)
            {
                unit.ChangeUnitState(EnemyUnitState.Attack);
            }
        }
        else if (unit.CurrentAction == ActionType.SKill)
        {
            if (unit is PlayerUnitController)
            {
                unit.ChangeUnitState(PlayerUnitState.Skill);
            }
            else if (unit is EnemyUnitController)
            {
                unit.ChangeUnitState(EnemyUnitState.Skill);
            }

            if (unit.CurrentAttackAction.ActionSo is RangeSkillSO rangeSkill)
            {
                if (!rangeSkill.IsProjectile)
                {
                    unit.OnSkillFinished += onReactionEndHandler;
                }
            }
            else
            {
                unit.OnSkillFinished += onReactionEndHandler;
            }
        }
    }

    public void OnUpdate(Unit unit)
    {
    }

    public void OnExit(Unit unit)
    {
        unit.OnHitFinished -= onReactionEndHandler;

        action = null;
        handler = null;
        target = null;
        onReactionEndHandler = null;
    }

    private void ProceedToNextState(Unit unit)
    {
        if (unit.IsDead)
        {
            unit.ChangeTurnState(TurnStateType.EndTurn);
            return;
        }

        Debug.Log($"{unit.name} ProceedToNextState");

        if (unit.CurrentAttackAction.DistanceType == AttackDistanceType.Melee)
        {
            unit.ChangeTurnState(TurnStateType.Return);
        }
        else
        {
            unit.ChangeTurnState(TurnStateType.EndTurn);
        }
    }
}

public class ReturnState : ITurnState
{
    public void OnEnter(Unit unit)
    {
        //되돌아가는 함수
        if (unit is PlayerUnitController)
        {
            unit.ChangeUnitState(PlayerUnitState.Return);
        }
        else if (unit is EnemyUnitController)
        {
            unit.ChangeUnitState(EnemyUnitState.Return);
        }
    }

    public void OnUpdate(Unit unit)
    {
    }

    public void OnExit(Unit unit)
    {
    }
}

public class EndTurnState : ITurnState
{
    public void OnEnter(Unit unit)
    {
        Debug.Log("EndTurn State");
        unit.StartCoroutine(DelayEndTurn(unit));
    }

    public void OnUpdate(Unit unit)
    {
    }

    public void OnExit(Unit unit)
    {
    }

    private IEnumerator DelayEndTurn(Unit unit)
    {
        yield return new WaitForFixedUpdate();
        unit.EndTurn();
    }
}