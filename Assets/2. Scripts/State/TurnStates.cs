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
    private Action onActSequenceEndHandler;
    private bool hasAdvanced;
    private bool subscribedSkillFinished;

    public void OnEnter(Unit unit)
    {
        hasAdvanced = false;
        subscribedSkillFinished = false;

        Unit target = unit.Target as Unit;
        if (target != null)
        {
            target.SetLastAttacker(unit);
        }

        onActSequenceEndHandler = () =>
        {
            if (hasAdvanced)
            {
                return;
            }

            hasAdvanced = true;
            ProceedToNextState(unit);
        };

        // 대상의 피격 리액션이 끝나면 진행(패턴 상 공격자 OnHitFinished가 호출되도록 연결되어 있음)
        unit.OnHitFinished += onActSequenceEndHandler;

        if (unit.CurrentAction == ActionType.Attack)
        {
            unit.EnterAttackState();
        }
        else if (unit.CurrentAction == ActionType.Skill)
        {
            unit.EnterSkillState();

            // 논-프로젝타일은 스킬 종료 시점도 함께 기다림
            try
            {
                if (unit.CurrentAttackAction != null && unit.CurrentAttackAction.ActionSo is RangeSkillSO range)
                {
                    if (!range.IsProjectile)
                    {
                        unit.OnSkillFinished += onActSequenceEndHandler;
                        subscribedSkillFinished = true;
                    }
                }
                else
                {
                    unit.OnSkillFinished += onActSequenceEndHandler;
                    subscribedSkillFinished = true;
                }
            }
            catch
            {
                /* ActionSo 미구현 케이스 대비 */
            }
        }
    }

    public void OnUpdate(Unit unit)
    {
    }

    public void OnExit(Unit unit)
    {
        if (onActSequenceEndHandler != null)
        {
            unit.OnHitFinished -= onActSequenceEndHandler;
            if (subscribedSkillFinished)
            {
                unit.OnSkillFinished -= onActSequenceEndHandler;
            }
        }

        onActSequenceEndHandler = null;
        subscribedSkillFinished = false;
        hasAdvanced = false;
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