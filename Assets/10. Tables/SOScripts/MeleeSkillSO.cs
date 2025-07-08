using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeSkillSO", menuName = "ScriptableObjects/SKillType/Melee", order = 0)]
public class MeleeSkillSO : CombatActionSo
{
    public override void Execute(IAttackable attacker, IDamageable target)
    {
        //이펙트 추가
        // TargetSelect targetSelect = new TargetSelect(target as Unit, attacker as Unit);

        // foreach (var effect in attacker.SkillController.CurrentSkillData.skillEffect.skillEffectDatas)
        // {
        //     attacker.SkillController.targets = targetSelect.FindTargets(effect.selectTarget, effect.selectCamp);
        //     foreach (Unit unit in attacker.SkillController.targets)
        //     {
        //         effect.AffectTargetWithSkill(unit);
        //     }
        // }

        foreach (var effect in attacker.SkillController.CurrentSkillData.skillEffect.skillEffectDatas)
        {
            List<IDamageable> targets = attacker.SkillController.SkillSubTargets[effect];
            foreach (var subTarget in targets)
            {
                if(subTarget.IsDead) continue;
                effect.AffectTargetWithSkill(subTarget);
            }
        }
        
        
    }

    public override AttackDistanceType DistanceType => AttackDistanceType.Melee;
}