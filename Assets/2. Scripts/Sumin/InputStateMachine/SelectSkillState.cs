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
        if (context.SelectedSkill != null)
        {
            inputStateMachine.ChangeState<SelectTargetState>();
        }
    }

    public void Exit() { }
}
