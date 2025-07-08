using System;
using System.Buffers;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeSkillSO", menuName = "ScriptableObjects/SKillType/Range", order = 0)]
public class RangeSkillSO : RangeActionSo
{
    public string subProjectilePoolID;

    public override AttackDistanceType DistanceType => AttackDistanceType.Range;
    public override CombatActionSo     ActionSo     => this;

    public override void Execute(IAttackable attacker, IDamageable target)
    {
        // var skillController = attacker.SkillController;
        //
        // TargetSelect targetSelect = new TargetSelect(target as Unit, attacker as Unit);
        //
        // foreach (var effect in skillController.CurrentSkillData.skillEffect.skillEffectDatas)
        // {
        //     skillController.targets = targetSelect.FindTargets(effect.selectTarget, effect.selectCamp);
        //     foreach (Unit unit in skillController.targets)
        //     {
        //         if (unit == null) continue;
        //         ProjectileComponent = ObjectPoolManager.Instance.GetObject(effect.projectileID).GetComponent<SkillProjectile>();
        //         ProjectileComponent.Initialize(effect, skillController.SkillManager.Owner.GetCenter(), unit.GetCenter(), unit);
        //     }
        // }
        //
        // if (ProjectileComponent != null)
        // {
        //     ProjectileComponent.trigger.OnTriggerTarget += ResetProjectile;
        // }
        var skillController = attacker.SkillController;
        foreach (var effect in attacker.SkillController.CurrentSkillData.skillEffect.skillEffectDatas)
        {
            List<IDamageable> targets = attacker.SkillController.SkillSubTargets[effect];
            if(targets == null) return;
            foreach (var subTarget in targets)
            {
                if(subTarget.IsDead) continue;
                if (effect.projectileID != String.Empty)
                {
                    ProjectileComponent = ObjectPoolManager.Instance.GetObject(effect.projectileID)
                        .GetComponent<SkillProjectile>();
                    ProjectileComponent.Initialize(effect, skillController.SkillManager.Owner.GetCenter(),
                        target.Collider.bounds.center, target);
                }
                else
                {
                  effect.AffectTargetWithSkill(subTarget);
                }
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