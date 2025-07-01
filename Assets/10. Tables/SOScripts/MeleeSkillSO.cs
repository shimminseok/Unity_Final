using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeSkillSO", menuName = "ScriptableObjects/SKillType/Melee", order = 0)]
public class MeleeSkillSO : SkillTypeSO
{
    public override void UseSkill(BaseSkillController controller)
    {

        //SkillTypeSO에 있는 UseSKill 진짜 UseSkill
        this.skillController = controller;
        TargetSelect targetSelect = new TargetSelect(skillController.mainTarget);

        foreach (var effect in controller.CurrentSkillData.skillEffect.skillEffectDatas)
        {
            controller.targets = targetSelect.FindTargets(effect.selectTarget,effect.selectCamp);
            foreach (Unit target in controller.targets)
            {
                effect.AffectTargetWithSkill(target);
            }
        }
        //이펙트 추가
    }

    public override AttackDistanceType DistanceType => AttackDistanceType.Melee;
}