using UnityEngine;
using UnityEngine.Serialization;

public class RangeActionSo : CombatActionSo
{
    public GameObject projectilePrefab;

    public PoolableProjectile ProjectileComponent { get; protected set; }

    public bool IsProjectile => projectilePrefab != null;

    public override AttackDistanceType DistanceType => AttackDistanceType.Range;

    public override void Execute(Unit attacker, IDamageable target)
    {
    }
}