// 커맨드 패턴 사용하여 플레이어 전략 페이즈 구현
// 플레이어 전략 페이즈 순서
// 1. 유닛을 선택 
// 2. 선택한 유닛의 행동(기본공격/스킬1~3)을 선택 
// 3. 행동의 타겟 선택 (적/에너미/전체 등등등 스킬에 따라 다름)
// 4. 각 유닛의 행동과 타겟 저장 -> ActionCommandQueue
// 5. 1~4 과정을 반복하여 각 유닛의 행동을 정의하고 Start 누르면 BattleManager? TurnHandler?에게 넘겨줘서 순차적으로 실행

// 플레이어 입력 (전략 페이즈)
// ActionPlanner에 각 유닛의 IActionCommand 저장
// BattleManager가 ActionCommand 목록을 TurnHandler에 전달
// TurnHandler가 속도 순 정렬 → 한 명씩 Execute()

// 행동 커맨드 인터페이스

public interface IActionCommand
{
    Unit Executer { get; }
    void Execute();
}

// 기본 공격 커맨드
public class AttackCommand : IActionCommand
{
    public Unit Executer { get; private set; }
    private Unit target;

    public AttackCommand(Unit executer, Unit target)
    {
        this.Executer = executer;
        this.target = target;
    }


    public void Execute()
    {
        // 기본공격 실행
        Executer.Attack();
    }
}