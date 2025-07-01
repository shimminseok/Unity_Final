public interface IAttackable
{
    StatBase           AttackStat { get; }
    public IDamageable Target     { get; }
    public void        Attack();

    public void SetTarget(Unit target);
}