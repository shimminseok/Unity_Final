// 유닛의 행동 선택
using System.Diagnostics;

public class SelectSkillState : IInputState
{
    private readonly InputContext context;
    private readonly InputStateMachine inputStateMachine;

    public SelectSkillState(InputContext context, InputStateMachine inputStateMachine)
    {
        this.context = context;
        this.inputStateMachine = inputStateMachine;
    }

    public void Enter() { }

    public void HandleInput() 
    {
        // 스킬이 선택되면 타겟 선택 상태로 전환
        // TODO : 추후 스킬 꾹 누르면 상세정보 표시 등 고려.
        // 추가 UI 기능 구현 등이 없으면 해당 State는 불필요, ChangeState 각 버튼에 부여.
        if (context.SelectedSkill != null)
        {
            inputStateMachine.ChangeState<SelectTargetState>();
        }
    }

    public void Exit() { }
}
