using System.Collections.Generic;
using System.Diagnostics;

// 플레이어 유닛 선택
public class SelectExecuterState : IInputState
{
    private readonly InputContext context;
    private readonly UnitSelector selector;
    private readonly InputStateMachine inputStateMachine;

    public SelectExecuterState(InputContext context, UnitSelector selector, InputStateMachine inputStateMachine)
    {
        this.context = context;
        this.selector = selector;
        this.inputStateMachine = inputStateMachine;
    }

    public void Enter()
    {
        // 이전에 선택해뒀던게 있으면 Indicator 지우기
        if (context.SelectedExcuter != null)
        {
            context.SelectedExcuter.ToggleSelectedIndicator(false);
        }

        // 선택 가능한 플레이어 유닛 표시
        selector.ShowSelectableUnits(context.PlayerUnitLayer, true);
        
        // 유닛 선택하기 전까지는 SkillUI 꺼둠
        context.CloseSkillUI?.Invoke();
    }

    public void HandleInput()
    {
        if (selector.TrySelectUnit(context.PlayerUnitLayer, out ISelectable unit))
        {
            // Unit Select하면 context의 SelectedUnit에 넘겨줌
            context.SelectedExcuter = unit;
            Debug.Print("플레이어 선택");

            // 인디케이터 표시 전환
            selector.ShowSelectableUnits(context.PlayerUnitLayer, false);
            context.SelectedExcuter.PlaySelectEffect();
            context.SelectedExcuter.ToggleSelectedIndicator(true);

            // SkillUI는 켜주고 StartButton은 꺼주기
            context.CloseStartButtonUI?.Invoke();
            context.OpenSkillUI?.Invoke(context.SelectedExcuter.SelectedUnit);

            // 스킬 선택 상태로 넘어감
            inputStateMachine.ChangeState<SelectSkillState>();
        }
    }

    public void Exit() {}
}

