using UnityEngine.Serialization;

public class RangeActionSo : CombatActionSo
{
    public string projectilePoolId;

    public SkillProjectile ProjectileComponent { get; protected set; }

    public bool IsProjectile => projectilePoolId != string.Empty;

    public override AttackDistanceType DistanceType => AttackDistanceType.Range;

    public override void Execute(Unit attacker, IDamageable target)
    {
    }
}