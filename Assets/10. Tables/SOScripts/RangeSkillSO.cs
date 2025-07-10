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
        var skillController = attacker.SkillController;
        foreach (var effect in skillController.CurrentSkillData.skillEffect.skillEffectDatas)
        {
            List<IDamageable> targets = skillController.SkillSubTargets[effect];
            foreach (IDamageable unit in targets)
            {
                if (unit == null) continue;
                if (effect.projectilePrefab != null)
                {   string projectileID = effect.projectilePrefab.GetComponent<PoolableProjectile>().PoolID;
                    GameObject projectile = ObjectPoolManager.Instance.GetObject(projectileID);
                    if (projectile == null)
                        projectile = Instantiate(effect.projectilePrefab);
                    ProjectileComponent = projectile.GetComponent<PoolableProjectile>();
                    ProjectileComponent.Initialize(effect, skillController.SkillManager.Owner.GetCenter(), unit.Collider.bounds.center, unit);

                }
                else
                {
                    effect.AffectTargetWithSkill(unit as Unit);
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