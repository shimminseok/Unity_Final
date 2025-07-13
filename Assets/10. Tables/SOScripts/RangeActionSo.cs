using UnityEngine;
using UnityEngine.Serialization;

public class RangeActionSo : CombatActionSo
{
    public string projectilePoolID;
    public GameObject projectilePrefab;

    public PoolableProjectile ProjectileComponent { get; protected set; }

    public bool IsProjectile => projectilePrefab != null;

    public override AttackDistanceType DistanceType => AttackDistanceType.Range;

    public override void Execute(Unit attacker, IDamageable target)
    {
        foreach (var data in attacker.SkillController.CurrentSkillData.skillSo.skillEffect.skillEffectDatas)
        {
            foreach (var vfxData in data.skillVFX)
            {
                if (vfxData.type == VFXType.Cast)
                {
                    PoolableVFX vfx = VFXController.InstantiateVFX(vfxData.VFXPoolID, vfxData.VFXPrefab);
                    vfxData.Attacker = attacker.SkillManager.Owner;
                    vfxData.Target = target;
                    vfx.SetData(vfxData);
                    vfx.OnSpawnFromPool();
                }
            }
        }
    }
}