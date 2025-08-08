using PixPlays.ElementalVFX;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "NewRangeSkillSO", menuName = "ScriptableObjects/SKillType/Range", order = 0)]
public class RangeSkillSO : RangeActionSo
{
    public override AttackDistanceType DistanceType => AttackDistanceType.Range;
    public override CombatActionSo     ActionSo     => this;

    public override void Execute(IAttackable attacker, IDamageable target)
    {
        BaseSkillController skillController = attacker.SkillController;
        SkillData           currentSkill    = skillController.CurrentSkillData;
        PlaySFX(attacker);
        // PlayableAsset timeline = currentSkill.skillSo.skillTimeLine;
        // if (timeline != null)
        // {
        //     TimeLineManager.Instance.director.Play(timeline);
        // }

        foreach (SkillEffectData effect in currentSkill.Effect.skillEffectDatas)
        {
            List<IDamageable> targets = skillController.SkillSubTargets[effect];
            foreach (IDamageable unit in targets)
            {
                if (unit == null || unit.IsDead)
                {
                    continue;
                }

                if (effect.projectilePrefab != null)
                {
                    GameObject projectile = ObjectPoolManager.Instance.GetObject(effect.projectilePoolID);
                    if (projectile == null)
                    {
                        projectile = Instantiate(effect.projectilePrefab);
                    }

                    ProjectileComponent = projectile.GetComponent<PoolableProjectile>();
                    ProjectileComponent.Initialize(attacker, effect, attacker.Collider.bounds.center, unit.Collider.bounds.center, unit);
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