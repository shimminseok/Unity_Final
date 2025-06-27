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