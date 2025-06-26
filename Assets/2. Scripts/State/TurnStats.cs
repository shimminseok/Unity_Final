using System;
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

    public void Initialize(Unit unit)
    {
        owner = unit;
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
                    unit.TurnStateMachine.ChangeState(new MoveToTargetState());
                }
                else
                {
                    unit.TurnStateMachine.ChangeState(new ActState());
                }

                break;
            case ActionType.SKill:
                if (unit is PlayerUnitController player)
                {
                    if (player.PlayerSkillController.currentSkill.skillType is MeleeSkillSO)
                    {
                        unit.TurnStateMachine.ChangeState(new MoveToTargetState());
                    }
                    else if (player.PlayerSkillController.currentSkill.skillType is RangeSkillSO)
                    {
                        unit.TurnStateMachine.ChangeState(new ActState());
                    }
                }

                break;
            case ActionType.None:
                unit.EndTurn();
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
        if (unit is PlayerUnitController)
            unit.ChangeUnitState(PlayerUnitState.Move);
        else if (unit is EnemyUnitController)
            unit.ChangeUnitState(EnemyUnitState.Move);
    }

    public void OnUpdate(Unit unit)
    {
        if ((unit as IUnitFsmControllable)?.IsAtTargetPosition ?? false)
            unit.TurnStateMachine.ChangeState(new ActState());
    }

    public void OnExit(Unit unit)
    {
    }
}

public class ActState : ITurnState
{
    public void OnEnter(Unit unit)
    {
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
                enemy.ChangeUnitState(EnemyUnitState.Attack); // Enemy에 Skill 상태가 없으면 Attack으로 재사용
            }
        }
    }

    public void OnUpdate(Unit unit)
    {
        Debug.Log("Act가 도는중");
        if (!(unit as IUnitFsmControllable)?.IsAnimationDone ?? false)
            return;


        if (unit.CurrentAttackAction.DistanceType == AttackDistanceType.Melee)
        {
            Debug.Log("다시 내 집으로!!");
            unit.TurnStateMachine.ChangeState(new ReturnState());
        }
        else
        {
            Debug.Log("EndTurn");
            unit.TurnStateMachine.ChangeState(new EndTurnState());
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
            unit.TurnStateMachine.ChangeState(new EndTurnState());
    }

    public void OnExit(Unit unit)
    {
    }
}

public class EndTurnState : ITurnState
{
    public void OnEnter(Unit unit)
    {
        BattleManager.Instance.TurnHandler.OnUnitTurnEnd();
    }

    public void OnUpdate(Unit unit)
    {
    }

    public void OnExit(Unit unit)
    {
    }
}