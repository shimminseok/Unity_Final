public class SkillGachaHandler : IGachaHandler
{
    private readonly SkillGachaSystem gachaSystem;
    private readonly SkillGachaResultUI resultUI;
    private readonly UIManager uiManager;

    public SkillGachaHandler(SkillGachaSystem skillGachaSystem, SkillGachaResultUI skillResultUI)
    {
        this.gachaSystem = skillGachaSystem;
        this.resultUI = skillResultUI;
        uiManager = UIManager.Instance;
    }

    public bool CanDraw(int count)
    {
        return gachaSystem.CheckCanDraw(count);
    }

    public int GetDrawCost()
    {
        return gachaSystem.DrawCost;
    }

    public string GetGachaTypeName()
    {
        return "스킬 소환";
    }

    public void DrawAndDisplayResult(int count)
    {
        GachaResult<ActiveSkillSO>[] skills = gachaSystem.DrawSkills(count);

        uiManager.Open(resultUI);
        resultUI.ShowSkills(skills);
    }
}
