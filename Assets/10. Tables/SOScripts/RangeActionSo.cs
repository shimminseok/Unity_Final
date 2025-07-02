public class RangeActionSo : CombatActionSo
{
    public string projectilePoolId;

    public          SkillProjectile    ProjectileComponent { get; protected set; }
    public override AttackDistanceType DistanceType        => AttackDistanceType.Range;

    public override void Execute(Unit attacker, IDamageable target)
    {
    }
}