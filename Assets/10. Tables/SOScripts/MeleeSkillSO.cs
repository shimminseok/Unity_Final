using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeSkillSO", menuName = "ScriptableObjects/SKillType/Melee", order = 0)]
public class MeleeSkillSO : SkillTypeSO
{
    public override void UseSkill(BaseSkillController controller)
    {
        this.skillController = controller;
        if (controller.mainTarget != null)
        {
            controller.CurrentSkillData.mainEffect.AffectTargetWithSkill(controller.mainTarget);
        }


        if (controller.subTargets != null)
        {
            foreach (Unit subTarget in controller.subTargets)
            {
                controller.CurrentSkillData.subEffect.AffectTargetWithSkill(subTarget);
            }
        }
    }

    public override AttackDistanceType DistanceType => AttackDistanceType.Melee;
}