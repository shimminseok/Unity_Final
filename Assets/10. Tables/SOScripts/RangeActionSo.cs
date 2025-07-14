using UnityEngine;
using UnityEngine.Serialization;

public class RangeActionSo : CombatActionSo
{
    public string projectilePoolID;
    public GameObject projectilePrefab;

    public PoolableProjectile ProjectileComponent { get; protected set; }

    public bool IsProjectile => projectilePrefab != null;

    public override AttackDistanceType DistanceType => AttackDistanceType.Range;

    public override void Execute(IAttackable attacker, IDamageable target)
    {
        foreach (var data in attacker.SkillController.CurrentSkillData.skillSo.buffEffect.skillEffectDatas)
        {
            VFXController.VFXListPlay(data.skillVFX,VFXType.Cast,VFXSpawnReference.Target, target as IEffectProvider,true);
            VFXController.VFXListPlay(data.skillVFX,VFXType.Cast,VFXSpawnReference.Caster, attacker as IEffectProvider,true);
        }
        
    }
}