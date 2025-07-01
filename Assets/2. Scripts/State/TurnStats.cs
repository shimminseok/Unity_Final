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
    private bool waitOneFrame = false;

    public void OnEnter(Unit unit)
    {
        waitOneFrame = false;

        if (unit is PlayerUnitController)
            unit.ChangeUnitState(PlayerUnitState.Move);
        else if (unit is EnemyUnitController)
            unit.ChangeUnitState(EnemyUnitState.Move);
    }

    public void OnUpdate(Unit unit)
    {
        if (!waitOneFrame)
        {
            waitOneFrame = true;
            return;
        }

        if ((unit as IUnitFsmControllable)?.IsAtTargetPosition ?? false)
            unit.ChangeTurnState(TurnStateType.Act);
    }

    public void OnExit(Unit unit)
    {
    }
}

public class ActState : ITurnState
{
    private bool waitOneFrame = false;

    public void OnEnter(Unit unit)
    {
        waitOneFrame = false;
        if (unit.CurrentAction == ActionType.Attack)
        {
            if (unit is PlayerUnitController player)
            {
                player.ChangeUnitState(PlayerUnitState.Attack);
            }
            else if (unit is EnemyUnitController enemy)
            {
                enemy.ChangeUnitState(EnemyUnitState.Attack);
            }
        }
        else if (unit.CurrentAction == ActionType.SKill)
        {
            if (unit is PlayerUnitController player)
            {
                player.ChangeUnitState(PlayerUnitState.Skill);
            }
            else if (unit is EnemyUnitController enemy)
            {
                enemy.ChangeUnitState(EnemyUnitState.Skill);
            }
        }
    }

    public void OnUpdate(Unit unit)
    {
        if (!waitOneFrame)
        {
            waitOneFrame = true;
            return;
        }

        if (!(unit as IUnitFsmControllable)?.IsAnimationDone ?? false)
            return;


        if (unit.CurrentAttackAction.DistanceType == AttackDistanceType.Melee)
        {
            unit.ChangeTurnState(TurnStateType.Return);
        }
        else
        {
            unit.ChangeTurnState(TurnStateType.EndTurn);
        }
    }

    public void OnExit(Unit unit)
    {
    }
}

public class ReturnState : ITurnState
{
    public void OnEnter(Unit unit)
    {
        //되돌아가는 함수
        if (unit is PlayerUnitController)
            unit.ChangeUnitState(PlayerUnitState.Return);
        else if (unit is EnemyUnitController)
            unit.ChangeUnitState(EnemyUnitState.Return);
    }

    public void OnUpdate(Unit unit)
    {
        if ((unit as IUnitFsmControllable)?.IsAtTargetPosition ?? false)
            unit.ChangeTurnState(TurnStateType.EndTurn);
    }

    public void OnExit(Unit unit)
    {
    }
}

public class EndTurnState : ITurnState
{
    private bool waitOneFrame = false;

    public void OnEnter(Unit unit)
    {
        waitOneFrame = false;
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