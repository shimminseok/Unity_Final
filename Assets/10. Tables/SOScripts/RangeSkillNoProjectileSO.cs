using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeNoProjectileSkillSO", menuName = "ScriptableObjects/SKillType/RangeNoProjectile", order = 0)]
public class RangeSkillNoProjectileSO : RangeActionSo
{
    public override AttackDistanceType DistanceType => AttackDistanceType.RangeNoProjectile;
    public override CombatActionSo     ActionSo     => this;

    public override void Execute(IAttackable attacker, IDamageable target)
    {
        foreach (var effect in attacker.SkillController.CurrentSkillData.BuffEffect.skillEffectDatas)
        {
            List<IDamageable> targets = attacker.SkillController.SkillSubTargets[effect];
            foreach (var subTarget in targets)
            {
                if(subTarget.IsDead) continue;
                effect.AffectTargetWithSkill(subTarget as Unit);
            }
        }
    }

}
