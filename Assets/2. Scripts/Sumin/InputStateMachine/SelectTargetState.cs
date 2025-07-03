using System.Collections.Generic;
using UnityEngine;

// 유닛의 타겟 선택
public class SelectTargetState : IInputState
{
    private readonly InputContext context;
    private readonly UnitSelector selector;
    private readonly InputStateMachine inputStateMachine;

    public SelectTargetState(InputContext context, UnitSelector selector, InputStateMachine inputStateMachine)
    {
        this.context = context;
        this.selector = selector;
        this.inputStateMachine = inputStateMachine;
    }

    public void Enter(){}

    public void HandleInput()
    {
        if (selector.TrySelectUnit(context.TargetLayer, out ISelectable target))
        {
            // Unit Select하면 context의 SelectedTarget에 넘겨줌
            context.SelectedTarget = target;

            Unit executerUnit = context.SelectedExcuter.SelectedUnit;
            Unit targetUnit = context.SelectedTarget.SelectedUnit;

            // 선택 이펙트
            targetUnit.PlaySelectEffect();

            // executerUnit의 타겟 지정하여 전달
            executerUnit.SetTarget(targetUnit);

            // 커맨드 생성
            context.PlanActionCommand?.Invoke(executerUnit, targetUnit, context.SelectedSkill);

            // 선택 단계로 이동
            inputStateMachine.ChangeState<SelectExecuterState>();

            // 인디케이터 표시 전환
            selector.ShowSelectableUnits(context.TargetLayer, false);
            context.SelectedExcuter.ToggleSelectedIndicator(false);

            // Start 버튼 활성화
            context.OpenStartButtonUI?.Invoke();
        }
    }

    public void Exit() {}
}

