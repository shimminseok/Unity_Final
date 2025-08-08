using UnityEngine;

public abstract class SkillTypeSO : ScriptableObject, IAttackAction
{
    protected BaseSkillController skillController;
    public abstract void UseSkill(BaseSkillController skillController);

    public abstract AttackDistanceType DistanceType { get; }
    public          CombatActionSo     ActionSo     { get; }
}