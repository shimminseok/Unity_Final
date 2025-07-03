// 행동 커맨드 인터페이스
public interface IActionCommand
{
    Unit Executer { get; }
    Unit Target { get; }
    public SkillData SkillData { get; }
}

// 액션 커맨드 저장
public class ActionCommand : IActionCommand
{
    public Unit Executer { get; private set; }
    public Unit Target { get; private set; }

    public SkillData SkillData { get; private set; }
    public ActionType ActionType => SkillData == null ? ActionType.Attack : ActionType.SKill;

    public ActionCommand(Unit executer, Unit target, SkillData skillData = null)
    {
        this.Executer = executer;
        this.Target = target;
        this.SkillData = skillData;
    }


}