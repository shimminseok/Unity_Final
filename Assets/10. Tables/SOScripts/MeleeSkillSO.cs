using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeSkillSO", menuName = "ScriptableObjects/SKillType/Melee", order = 0)]
public class MeleeSkillSO : CombatActionSo
{
    public override void Execute(IAttackable attacker, IDamageable target)
    {
        foreach (var data in attacker.SkillController.CurrentSkillData.skillSo.buffEffect.skillEffectDatas)
        {
            VFXController.VFXListPlay(data.skillVFX,VFXType.Cast,VFXSpawnReference.Target, target as IEffectProvider,true);
            VFXController.VFXListPlay(data.skillVFX,VFXType.Cast,VFXSpawnReference.Caster, attacker as IEffectProvider,true);
        }

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

    public override AttackDistanceType DistanceType => AttackDistanceType.Melee;
}