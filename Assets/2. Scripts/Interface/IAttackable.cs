public interface IAttackable
{
    StatBase           AttackStat { get; }
    public IDamageable Target     { get; }
    public IDamageable CounterTarget { get; }

    BaseSkillController SkillController { get; }
    
    public bool IsCounterAttack { get; }
    public void        Attack();

    public void SetTarget(IDamageable target);
}