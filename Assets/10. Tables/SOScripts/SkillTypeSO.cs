using UnityEngine;

public abstract class SkillTypeSO : ScriptableObject
{
    protected BaseSkillController skillController;
    public abstract void UseSkill(BaseSkillController skillController);

}
