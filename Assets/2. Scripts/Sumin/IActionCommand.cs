// 행동 커맨드 인터페이스
using UnityEditor.Rendering.LookDev;

public interface IActionCommand
{
    Unit Executer { get; }
    Unit Target { get; }
    public SkillData SkillData { get; }

    void Execute();
}

// 액션 커맨드 저장
public class ActionCommand : IActionCommand
{
    public Unit Executer { get; private set; }
    public Unit Target { get; private set; }

    public SkillData SkillData { get; private set; }

    public ActionCommand(Unit executer, Unit target, SkillData skillData = null)
    {
        this.Executer = executer;
        this.Target = target;
        this.SkillData = skillData;
    }
    
    // 유닛이 할 행동 커맨드를 저장할 때 유닛에게 반영해줌.
    public void Execute()
    {
        if (SkillData == null)
        {
            Executer.ChangeAction(ActionType.Attack);
        }
        else
        {
            Executer.SkillController.ChangeSkill(Executer.SkillController.GetSkillIndex(SkillData));
            Executer.ChangeAction(ActionType.SKill);
        }
        Executer.SetTarget(Target);
    }
}