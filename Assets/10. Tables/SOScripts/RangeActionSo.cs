using System;
using UnityEngine;
using UnityEngine.Playables;

public class RangeActionSo : CombatActionSo
{
    [Header("투사체")]
    public string projectilePoolID;

    public GameObject projectilePrefab;

    public PoolableProjectile ProjectileComponent { get; protected set; }

    public virtual bool IsProjectile { get; protected set; }

    public override AttackDistanceType DistanceType => AttackDistanceType.Range;

    public override void Execute(IAttackable attacker, IDamageable target)
    {
    }

    public void CloneSkillType()
    {
    }

    public void SetIsProjectile(bool isProjectile)
    {
        IsProjectile = isProjectile;
    }
}