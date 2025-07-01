using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeSkillSO", menuName = "ScriptableObjects/SKillType/Melee", order = 0)]
public class MeleeSkillSO : SkillTypeSO
{
    public override void UseSkill(BaseSkillController controller)
    {
        //실질적인 데미지 주는 형식의 UseSkill
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

        //이펙트 추가
    }

    public override AttackDistanceType DistanceType => AttackDistanceType.Melee;
}