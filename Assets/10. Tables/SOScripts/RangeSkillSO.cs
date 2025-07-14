using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeSkillSO", menuName = "ScriptableObjects/SKillType/Range", order = 0)]
public class RangeSkillSO : RangeActionSo
{
    public override AttackDistanceType DistanceType => AttackDistanceType.Range;
    public override CombatActionSo     ActionSo     => this;

    public override void Execute(IAttackable attacker, IDamageable target)
    {
        base.Execute(attacker, target);
        var skillController = attacker.SkillController;
        foreach (var effect in skillController.CurrentSkillData.Effect.skillEffectDatas)
        {
            List<IDamageable> targets = skillController.SkillSubTargets[effect];
            foreach (IDamageable unit in targets)
            {
                if (unit == null) continue;
                if (effect.projectilePrefab != null)
                {   
                    GameObject projectile = ObjectPoolManager.Instance.GetObject(effect.projectilePoolID);
                    if (projectile == null)
                        projectile = Instantiate(effect.projectilePrefab);
                    ProjectileComponent = projectile.GetComponent<PoolableProjectile>();
                    ProjectileComponent.Initialize(effect, attacker.Collider.bounds.center, unit.Collider.bounds.center, unit);
                }
                else
                {
                    effect.AffectTargetWithSkill(unit);
                }
            }
        }

        if (ProjectileComponent != null)
        {
            ProjectileComponent.trigger.OnTriggerTarget += ResetProjectile;
        }
    }


    private void ResetProjectile()
    {
        ProjectileComponent = null;
    }
}