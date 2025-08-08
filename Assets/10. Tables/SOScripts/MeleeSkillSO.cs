using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeSkillSO", menuName = "ScriptableObjects/SKillType/Melee", order = 0)]
public class MeleeSkillSO : CombatActionSo
{
    public override void Execute(IAttackable attacker, IDamageable target)
    {
        PlaySFX(attacker);
        foreach (var effect in attacker.SkillController.CurrentSkillData.Effect.skillEffectDatas)
        {
            List<IDamageable> targets = attacker.SkillController.SkillSubTargets[effect];
            foreach (var subTarget in targets)
            {
                
                if (subTarget == null || subTarget.IsDead)
                {
                    continue;
                }
                effect.AffectTargetWithSkill(subTarget as Unit);
            }
        }
    }

    public override AttackDistanceType DistanceType => AttackDistanceType.Melee;
}