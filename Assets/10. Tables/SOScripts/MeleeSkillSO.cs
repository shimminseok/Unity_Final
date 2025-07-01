using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeSkillSO", menuName = "ScriptableObjects/SKillType/Melee", order = 0)]
public class MeleeSkillSO : CombatActionSo
{
    public override void Execute(Unit attacker)
    {
        //실질적인 데미지 주는 형식의 UseSkill
        var skillController = attacker.SkillController;
        if (skillController.mainTarget != null)
        {
            skillController.CurrentSkillData.mainEffect.AffectTargetWithSkill(skillController.mainTarget);
        }


        if (skillController.subTargets != null)
        {
            foreach (Unit subTarget in skillController.subTargets)
            {
                skillController.CurrentSkillData.subEffect.AffectTargetWithSkill(subTarget);
            }
        }

        //이펙트 추가
    }

    public override AttackDistanceType DistanceType => AttackDistanceType.Melee;
}