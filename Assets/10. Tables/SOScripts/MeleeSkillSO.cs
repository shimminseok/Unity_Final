using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeSkillSO", menuName = "ScriptableObjects/SKillType/Melee", order = 0)]
public class MeleeSkillSO : CombatActionSo
{
    public override void Execute(Unit attacker, IDamageable target)
    {
        foreach (var data in attacker.SkillController.CurrentSkillData.skillSo.buffEffect.skillEffectDatas)
        {
            VFXController.VFXListPlay(data.skillVFX,VFXType.Cast,VFXSpawnReference.Target, target as IEffectProvider,true);
            VFXController.VFXListPlay(data.skillVFX,VFXType.Cast,VFXSpawnReference.Caster, attacker as IEffectProvider,true);
        }

        foreach (var effect in attacker.SkillController.CurrentSkillData.BuffEffect.skillEffectDatas)
        {
            attacker.SkillController.targets = targetSelect.FindTargets(effect.selectTarget, effect.selectCamp);
            foreach (Unit unit in attacker.SkillController.targets)
            {
                effect.AffectTargetWithSkill(unit);
            }
        }
    }

    public override AttackDistanceType DistanceType => AttackDistanceType.Melee;
}