using UnityEngine;
using UnityEngine.Playables;

public class RangeActionSo : CombatActionSo
{
    public string projectilePoolID;
    public GameObject projectilePrefab;

    public PoolableProjectile ProjectileComponent { get; protected set; }

    public bool IsProjectile { get; private set; }

    public override AttackDistanceType DistanceType => AttackDistanceType.Range;

    public override void Execute(IAttackable attacker, IDamageable target)
    {
    }

    public void SetIsProjectile(bool isProjectile)
    {
        IsProjectile = isProjectile;
    }
}