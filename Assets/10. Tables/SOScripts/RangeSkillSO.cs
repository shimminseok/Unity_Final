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
        var skillController = attacker.SkillController;
        var currentSkill = skillController.CurrentSkillData;
        // PlayableAsset timeline = currentSkill.skillSo.skillTimeLine;
        // if (timeline != null)
        // {
        //     TimeLineManager.Instance.director.Play(timeline);
        // }
        
        foreach (var data in currentSkill.skillSo.effect.skillEffectDatas)
        {
            VFXController.VFXListPlay(data.skillVFX,VFXType.Cast,VFXSpawnReference.Target, target as IEffectProvider,true);
            VFXController.VFXListPlay(data.skillVFX,VFXType.Cast,VFXSpawnReference.Caster, attacker as IEffectProvider,true);
        }
        
        foreach (var effect in currentSkill.Effect.skillEffectDatas)
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