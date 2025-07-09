using System;
using System.Buffers;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeSkillSO", menuName = "ScriptableObjects/SKillType/Range", order = 0)]
public class RangeSkillSO : RangeActionSo
{
    public override AttackDistanceType DistanceType => AttackDistanceType.Range;
    public override CombatActionSo     ActionSo     => this;

    public override void Execute(IAttackable attacker, IDamageable target)
    {
        var skillController = attacker.SkillController;
        foreach (var effect in attacker.SkillController.CurrentSkillData.skillEffect.skillEffectDatas)
        {
            List<IDamageable> targets = attacker.SkillController.SkillSubTargets[effect];
            if(targets == null) return;
            foreach (var subTarget in targets)
            {
                if(subTarget.IsDead) continue;
                ProjectileComponent = ObjectPoolManager.Instance.GetObject(effect.projectileID).GetComponent<SkillProjectile>();
                ProjectileComponent.Initialize(effect, skillController.SkillManager.Owner.GetCenter(),target.Collider.bounds.center, target);

            }
            if (ProjectileComponent != null)
            {
                ProjectileComponent.trigger.OnTriggerTarget += ResetProjectile;
            }
        }
    }


    private void ResetProjectile()
    {
        ProjectileComponent = null;
    }
}