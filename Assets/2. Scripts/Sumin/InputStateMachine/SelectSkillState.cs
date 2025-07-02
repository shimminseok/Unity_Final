// 유닛의 행동 선택
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
        // 스킬 또는 기본 공격이 선택되면 타겟 선택 상태로 전환
        if (context.SelectedSkill != null || context.SelectedExcuter?.SelectedUnit?.CurrentAction == ActionType.Attack)
        {
            inputStateMachine.ChangeState<SelectTargetState>();
        }
    }

    public void Exit() { }
}
