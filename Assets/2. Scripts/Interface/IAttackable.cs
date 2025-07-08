public interface IAttackable
{
    StatBase           AttackStat { get; }
    public IDamageable Target     { get; }

    BaseSkillController SkillController { get; }
    
    
    public void        Attack();

    public void SetTarget(IDamageable target);
}